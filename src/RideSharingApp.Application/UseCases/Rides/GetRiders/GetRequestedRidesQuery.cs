using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.UseCases.Rides.RequestRiders;

namespace RideSharingApp.Application.UseCases.Rides.GetRiders;

public record GetRequestedRidesQuery(Guid PassengerId, string PickupLocation, string DropoffLocation) : IQuery<IEnumerable<RequestedRideResponse>>;