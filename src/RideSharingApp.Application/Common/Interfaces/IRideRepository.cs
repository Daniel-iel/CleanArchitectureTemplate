using RideSharingApp.Domain.Rides;

namespace RideSharingApp.Application.Common.Interfaces;

public interface IRideRepository
{
    Task<RideRequest> AddAsync(RideRequest ride, CancellationToken cancellationToken);
    Task<IEnumerable<RideRequest>> GetRequestedRidesAsync(CancellationToken cancellationToken);
    Task UpdateStatusAsync(
        Guid rideId,
        RideStatus status,
        Guid? driverId,
        CancellationToken cancellationToken
    );
}
