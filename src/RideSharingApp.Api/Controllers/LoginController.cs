using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.UseCases.Login;

namespace RideSharingApp.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class LoginController : ControllerBase
{
    private readonly ICommandHandler<LoginCommand, LoginResponse> _handler;
    public LoginController(ICommandHandler<LoginCommand, LoginResponse> handler)
    {
        _handler = handler;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(command, cancellationToken);
        if (result == null)
        {
            return Unauthorized();
        }
        return Ok(result);
    }
}
