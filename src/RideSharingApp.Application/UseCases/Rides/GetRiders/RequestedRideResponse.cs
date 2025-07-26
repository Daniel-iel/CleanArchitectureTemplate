

namespace RideSharingApp.Application.UseCases.Rides.GetRiders
{
    public sealed class RequestedRideResponse
    {
        public Guid Id { get; }
        public Guid PassengerId { get; }
        public string PickupLocation { get; }
        public string DropoffLocation { get; }
        public Guid? DriverId { get; }
        public decimal EstimatedCost { get; }
        public string Status { get; }
        public DateTime RequestedAt { get; }

        public RequestedRideResponse(Guid id, Guid passengerId, string pickupLocation, string dropoffLocation, Guid? driverId, decimal estimatedCost, string status, DateTime requestedAt)
        {
            Id = id;
            PassengerId = passengerId;
            PickupLocation = pickupLocation;
            DropoffLocation = dropoffLocation;
            DriverId = driverId;
            EstimatedCost = estimatedCost;
            Status = status;
            RequestedAt = requestedAt;
        }
    }
}