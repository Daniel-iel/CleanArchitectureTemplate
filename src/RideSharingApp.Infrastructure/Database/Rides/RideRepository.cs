using Dapper;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Domain.Rides;

namespace RideSharingApp.Infrastructure.Database.Rides;

public class RideRepository : IRideRepository
{
    private readonly IConnection _connection;

    public RideRepository(IConnection connection)
    {
        _connection = connection;
    }

    public async Task<RideRequest> AddAsync(RideRequest ride, CancellationToken cancellationToken)
    {
        ride.Id = Guid.NewGuid();

        const string query =
            """
                INSERT INTO Rides (Id, Passenger_Id, Pickup_Location, Dropoff_Location, Requested_At, Status)
                VALUES (@Id, @PassengerId, @PickupLocation, @DropoffLocation, @RequestedAt, @Status);
            """;

        var command = new CommandDefinition(
          commandText: query,
          parameters: ride,
          _connection.Transaction,
          cancellationToken: cancellationToken
        );

        await _connection.DbConnection.ExecuteAsync(command);

        return ride;
    }

    public Task<IEnumerable<RideRequest>> GetRequestedRidesAsync(CancellationToken cancellationToken)
    {
        const string query =
           """
               SELECT * FROM Rides WHERE Status = @Status;
           """;

        var parameters = new
        {
            Status = RideStatus.Requested
        };

        var command = new CommandDefinition(
          commandText: query,
          parameters: parameters,
          cancellationToken: cancellationToken
        );

        return _connection.DbConnection.QueryAsync<RideRequest>(command);
    }

    public Task UpdateStatusAsync(
        Guid rideId,
        RideStatus status,
        Guid? driverId,
        CancellationToken cancellationToken)
    {
        const string query =
          """
            UPDATE Rides SET Status = @Status, DriverId = @DriverId WHERE Id = @Id;
          """;

        var parameters = new
        {
            Status = status,
            DriverId = driverId,
            Id = rideId
        };

        var command = new CommandDefinition(
          commandText: query,
          parameters: parameters,
          _connection.Transaction,
          cancellationToken: cancellationToken
        );

        return _connection.DbConnection.ExecuteAsync(command);
    }
}
