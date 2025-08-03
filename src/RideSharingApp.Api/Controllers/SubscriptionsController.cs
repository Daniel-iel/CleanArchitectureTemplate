using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSharingApp.Application.UseCases.Subscription.Create;

namespace RideSharingApp.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class SubscriptionsController : ControllerBase
{
    private readonly CreateSubscriptionCommandHandler _handler;
    private readonly IValidator<CreateSubscriptionCommand> _validator;

    public SubscriptionsController(CreateSubscriptionCommandHandler handler, IValidator<CreateSubscriptionCommand> validator)
    {
        _handler = handler;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(command);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }
        var result = await _handler.HandleAsync(command, cancellationToken);
        return Ok(result);
    }
}
