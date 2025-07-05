namespace RideSharingApp.Application.UseCases.Rides.RequestRiders;

public record RequestedRideResponse(Guid RideId, Guid PassengerId, string PickupLocation, string DropoffLocation, string Status, DateTime RequestedAt);
