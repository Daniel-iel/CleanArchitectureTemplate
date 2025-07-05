using RideSharingApp.Domain.Rides;

namespace RideSharingApp.Application.Common.Interfaces;

public interface IRideRepository
{
    Task<RideRequest> AddAsync(RideRequest ride);
    Task<IEnumerable<RideRequest>> GetRequestedRidesAsync();
    Task UpdateStatusAsync(Guid rideId, RideStatus status, Guid? driverId = null);
}
