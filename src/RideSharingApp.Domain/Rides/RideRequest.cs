namespace RideSharingApp.Domain.Rides;

public class RideRequest
{
    public Guid Id { get; set; }
    public Guid PassengerId { get; set; }
    public string PickupLocation { get; set; } = string.Empty;
    public string DropoffLocation { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public Guid? DriverId { get; set; }
    public decimal EstimatedCost { get; set; } = 0.0m;
    public RideStatus Status { get; set; } = RideStatus.Requested;
}
