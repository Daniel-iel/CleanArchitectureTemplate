using RideSharingApp.Application.Common.DispacherEvent;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Application.Results;
using RideSharingApp.Domain.Rides;
using RideSharingApp.Infrastructure.Database;

namespace RideSharingApp.Application.UseCases.Rides.RequestRiders;

public sealed class RequestRideCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRideRepository _rideRepo;
    private readonly IUserRepository _userRepo;
    private readonly ISubscriptionRepository _subRepo;
    private readonly EventPublisher _eventPublisher;

    public RequestRideCommandHandler(
        IUnitOfWork unitOfWork,
        IRideRepository rideRepo,
        IUserRepository userRepo,
        ISubscriptionRepository subRepo,
        EventPublisher eventPublisher)
    {
        _unitOfWork = unitOfWork;
        _rideRepo = rideRepo;
        _userRepo = userRepo;
        _subRepo = subRepo;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result<RequestRideResponse>> Handle(RequestRideCommand command)
    {
        var user = await _userRepo.GetByIdAsync(command.PassengerId);
        if (user == null)
        {
            return Result.Failure<RequestRideResponse>(Error.NotFound("User.NotFound", "Usuário não encontrado."));
        }

        var subscriptions = await _subRepo.GetByUserIdAsync(user.Id);
        var activeSub = subscriptions.FirstOrDefault(s => s.IsActive);
        if (activeSub == null)
        {
            return Result.Failure<RequestRideResponse>(Error.Problem("Subscription.NoneActive", "Nenhuma assinatura ativa encontrada."));
        }

        try
        {
            var ride = new RideRequest
            {
                PassengerId = command.PassengerId,
                PickupLocation = command.PickupLocation,
                DropoffLocation = command.DropoffLocation,
                RequestedAt = DateTime.UtcNow,
                Status = RideStatus.Requested
            };

            var created = await _rideRepo.AddAsync(ride);

            var rideRequestedEvent = new RideRequestedEvent(created.Id, created.PassengerId, created.PickupLocation, created.DropoffLocation, created.RequestedAt);
            await _eventPublisher.PublishAsync(rideRequestedEvent);

            return Result.Success(new RequestRideResponse(created.Id, created.Status.ToString()));
        }
        catch (Exception ex)
        {
            _unitOfWork.BeginTransaction();

            return Result.Failure<RequestRideResponse>(Error.Problem("Ride.RequestFailed", $"Falha ao solicitar corrida: {ex.Message}"));
        }
    }
}
