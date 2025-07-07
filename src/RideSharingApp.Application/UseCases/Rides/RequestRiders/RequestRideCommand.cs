using RideSharingApp.Application.Abstractions.Messaging;

namespace RideSharingApp.Application.UseCases.Rides.RequestRiders;

public sealed record RequestRideCommand(
    Guid PassengerId,
    string PickupLocation,
    string DropoffLocation) : ICommand<RequestRideResponse>;
