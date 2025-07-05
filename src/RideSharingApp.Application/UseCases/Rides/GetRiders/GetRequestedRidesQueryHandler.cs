using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Application.UseCases.Rides.RequestRiders;

namespace RideSharingApp.Application.UseCases.Rides.GetRiders;

public class GetRequestedRidesQueryHandler
{
    private readonly IRideRepository _rideRepo;
    public GetRequestedRidesQueryHandler(IRideRepository rideRepo)
    {
        _rideRepo = rideRepo;
    }

    public async Task<IEnumerable<RequestedRideResponse>> HandleAsync(GetRequestedRidesQuery query)
    {
        var rides = await _rideRepo.GetRequestedRidesAsync();
        return rides.Select(r => new RequestedRideResponse(
            r.Id,
            r.PassengerId,
            r.PickupLocation,
            r.DropoffLocation,
            r.Status.ToString(),
            r.RequestedAt
        ));
    }
}
