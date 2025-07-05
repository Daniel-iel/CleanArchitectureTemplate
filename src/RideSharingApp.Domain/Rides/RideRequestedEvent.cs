namespace RideSharingApp.Domain.Rides;

public class RideRequestedEvent
{
    public Guid RideId { get; set; }
    public Guid PassengerId { get; set; }
    public string PickupLocation { get; set; }
    public string DropoffLocation { get; set; }
    public DateTime RequestedAt { get; set; }
    public RideRequestedEvent(Guid rideId, Guid passengerId, string pickup, string dropoff, DateTime requestedAt)
    {
        RideId = rideId;
        PassengerId = passengerId;
        PickupLocation = pickup;
        DropoffLocation = dropoff;
        RequestedAt = requestedAt;
    }
}
