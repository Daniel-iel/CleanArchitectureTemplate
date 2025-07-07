using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Application.Results;

namespace RideSharingApp.Application.UseCases.Rides.GetRiders;

public sealed class GetRequestedRidesQueryHandler : IQueryHandler<GetRequestedRidesQuery, IEnumerable<RequestedRideResponse>>
{
    private readonly IRideRepository _rideRepo;
    public GetRequestedRidesQueryHandler(IRideRepository rideRepo)
    {
        _rideRepo = rideRepo;
    }

    public async Task<Result<IEnumerable<RequestedRideResponse>>> HandleAsync(GetRequestedRidesQuery query, CancellationToken cancellationToken)
    {
        var rides = await _rideRepo.GetRequestedRidesAsync();
        if (rides == null || !rides.Any())
        {
            return Result.Failure<IEnumerable<RequestedRideResponse>>(Error.NotFound("Rides.NoneFound", "Nenhuma corrida encontrada."));
        }
        var response = rides.Select(r => new RequestedRideResponse(
            r.Id,
            r.PassengerId,
            r.PickupLocation,
            r.DropoffLocation,
            r.Status.ToString(),
            r.RequestedAt
        ));
        return Result.Success(response);
    }
}
