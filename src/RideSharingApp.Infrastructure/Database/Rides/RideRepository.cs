using Dapper;
using RideSharingApp.Domain.Rides;

namespace RideSharingApp.Infrastructure.Database.Rides;

using RideSharingApp.Application.Common.Interfaces;

public class RideRepository : IRideRepository
{
    private readonly IConnection connection;

    public RideRepository(IConnection connection)
    {
        this.connection = connection;
    }

    public async Task<RideRequest> AddAsync(RideRequest ride)
    {
        ride.Id = Guid.NewGuid();
        await connection.DbConnection.ExecuteAsync(
            "INSERT INTO Rides (Id, PassengerId, PickupLocation, DropoffLocation, RequestedAt, Status) VALUES (@Id, @PassengerId, @PickupLocation, @DropoffLocation, @RequestedAt, @Status)",
            ride,
            connection.Transaction
        );
        return ride;
    }

    public Task<IEnumerable<RideRequest>> GetRequestedRidesAsync()
    {
        return connection.DbConnection.QueryAsync<RideRequest>(
            "SELECT * FROM Rides WHERE Status = @Status",
            new { Status = RideStatus.Requested },
             connection.Transaction
        );
    }

    public Task UpdateStatusAsync(Guid rideId, RideStatus status, Guid? driverId = null)
    {
        return connection.DbConnection.ExecuteAsync(
            "UPDATE Rides SET Status = @Status, DriverId = @DriverId WHERE Id = @Id",
            new { Status = status, DriverId = driverId, Id = rideId },
             connection.Transaction
        );
    }
}
