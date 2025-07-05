using RideSharingApp.Application.Abstractions.Messaging;

namespace RideSharingApp.Application.UseCases.Rides.RequestRiders;

public record RequestRideCommand(Guid PassengerId, string PickupLocation, string DropoffLocation) : ICommand<RequestRideResponse>;
