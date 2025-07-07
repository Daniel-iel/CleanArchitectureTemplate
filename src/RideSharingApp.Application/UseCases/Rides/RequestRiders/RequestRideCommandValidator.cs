using FluentValidation;

namespace RideSharingApp.Application.UseCases.Rides.RequestRiders;

public sealed class RequestRideCommandValidator : AbstractValidator<RequestRideCommand>
{
    public RequestRideCommandValidator()
    {
        RuleFor(x => x.PassengerId).NotEmpty();
        RuleFor(x => x.PickupLocation).NotEmpty();
        RuleFor(x => x.DropoffLocation).NotEmpty();
    }
}
