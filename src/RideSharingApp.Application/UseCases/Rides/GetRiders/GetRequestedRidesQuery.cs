namespace RideSharingApp.Application.UseCases.Rides.GetRiders;

//public class GetRequestedRidesQuery { }
public record GetRequestedRidesQuery(Guid PassengerId, string PickupLocation, string DropoffLocation);