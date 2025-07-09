using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSharingApp.Application.UseCases.Subscription.Create;
using Asp.Versioning;

namespace RideSharingApp.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
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
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateSubscriptionCommand command, CancellationToken cancellationToken)
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
