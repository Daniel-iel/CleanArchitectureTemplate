using Dapper;
using RideSharingApp.Domain.Rides;

namespace RideSharingApp.Infrastructure.Database.Rides;

using RideSharingApp.Application.Common.Interfaces;

public class RideRepository : IRideRepository
{
    private readonly IUnitOfWork _unitOfWork;
    public RideRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<RideRequest> AddAsync(RideRequest ride)
    {
        ride.Id = Guid.NewGuid();
        await _unitOfWork.Connection.ExecuteAsync(
            "INSERT INTO Rides (Id, PassengerId, PickupLocation, DropoffLocation, RequestedAt, Status) VALUES (@Id, @PassengerId, @PickupLocation, @DropoffLocation, @RequestedAt, @Status)",
            ride,
            _unitOfWork.Transaction
        );
        return ride;
    }

    public Task<IEnumerable<RideRequest>> GetRequestedRidesAsync()
    {
        return _unitOfWork.Connection.QueryAsync<RideRequest>(
            "SELECT * FROM Rides WHERE Status = @Status",
            new { Status = RideStatus.Requested },
            _unitOfWork.Transaction
        );
    }

    public Task UpdateStatusAsync(Guid rideId, RideStatus status, Guid? driverId = null)
    {
        return _unitOfWork.Connection.ExecuteAsync(
            "UPDATE Rides SET Status = @Status, DriverId = @DriverId WHERE Id = @Id",
            new { Status = status, DriverId = driverId, Id = rideId },
            _unitOfWork.Transaction
        );
    }
}
