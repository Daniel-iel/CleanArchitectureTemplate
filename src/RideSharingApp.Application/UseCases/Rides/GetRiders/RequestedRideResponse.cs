
namespace RideSharingApp.Application.UseCases.Rides.GetRiders
{
    public sealed class RequestedRideResponse
    {
        private Guid id;
        private Guid passengerId;
        private string pickupLocation;
        private string dropoffLocation;
        private string v;
        private DateTime requestedAt;

        public RequestedRideResponse(Guid id, Guid passengerId, string pickupLocation, string dropoffLocation, string v, DateTime requestedAt)
        {
            this.id = id;
            this.passengerId = passengerId;
            this.pickupLocation = pickupLocation;
            this.dropoffLocation = dropoffLocation;
            this.v = v;
            this.requestedAt = requestedAt;
        }
    }
}