using FluentValidation;

namespace RideSharingApp.Application.UseCases.Subscription.Create;

public sealed class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
