using RideSharingApp.Application.Common.DispacherEvent;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Domain.Rides;

namespace RideSharingApp.Application.UseCases.Rides.RequestRiders;

public class RequestRideCommandHandler
{
    private readonly IRideRepository _rideRepo;
    private readonly IUserRepository _userRepo;
    private readonly ISubscriptionRepository _subRepo;
    private readonly EventPublisher _eventPublisher;

    public RequestRideCommandHandler(
        IRideRepository rideRepo,
        IUserRepository userRepo,
        ISubscriptionRepository subRepo,
        EventPublisher eventPublisher)
    {
        _rideRepo = rideRepo;
        _userRepo = userRepo;
        _subRepo = subRepo;
        _eventPublisher = eventPublisher;
    }

    public async Task<RideResponse> Handle(RequestRideCommand command)
    {
        var user = await _userRepo.GetByIdAsync(command.PassengerId);
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        var subscriptions = await _subRepo.GetByUserIdAsync(user.Id);
        var activeSub = subscriptions.FirstOrDefault(s => s.IsActive);
        if (activeSub == null)
        {
            throw new InvalidOperationException("No active subscription");
        }

        var ride = new RideRequest
        {
            PassengerId = command.PassengerId,
            PickupLocation = command.PickupLocation,
            DropoffLocation = command.DropoffLocation,
            RequestedAt = DateTime.UtcNow,
            Status = RideStatus.Requested
        };

        var created = await _rideRepo.AddAsync(ride);
        var evt = new RideRequestedEvent(created.Id, created.PassengerId, created.PickupLocation, created.DropoffLocation, created.RequestedAt);

        await _eventPublisher.PublishAsync(evt);
        return new RideResponse(created.Id, created.Status.ToString());
    }
}
