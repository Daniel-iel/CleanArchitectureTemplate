using RideSharingApp.SharedKernel.DomainEvents;

namespace RideSharingApp.Domain.Rides;

public record RideRequestedEvent(
    Guid RideId,
    Guid PassengerId,
    string PickupLocation,
    string DropoffLocation,
    DateTime RequestedAt) : IDomainEvent;
