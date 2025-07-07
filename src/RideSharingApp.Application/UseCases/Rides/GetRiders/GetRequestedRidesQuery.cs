using RideSharingApp.Application.Abstractions.Messaging;

namespace RideSharingApp.Application.UseCases.Rides.GetRiders;

public sealed record GetRequestedRidesQuery(
    Guid PassengerId,
    string PickupLocation,
    string DropoffLocation) : IQuery<IEnumerable<RequestedRideResponse>>;