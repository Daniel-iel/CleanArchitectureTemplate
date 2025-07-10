using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.UseCases.Rides.GetRiders;
using RideSharingApp.Application.UseCases.Rides.RequestRiders;

namespace RideSharingApp.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class RidesController : ControllerBase
{
    private readonly ICommandHandler<RequestRideCommand, RequestRideResponse> _requestHandler;
    private readonly IQueryHandler<GetRequestedRidesQuery, IEnumerable<RequestedRideResponse>> _getHandler;

    public RidesController(
        ICommandHandler<RequestRideCommand, RequestRideResponse> requestHandler,
        IQueryHandler<GetRequestedRidesQuery, IEnumerable<RequestedRideResponse>> getHandler)
    {
        _requestHandler = requestHandler;
        _getHandler = getHandler;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RequestRideAsync([FromBody] RequestRideCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _requestHandler.HandleAsync(command, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("requests")]
    [Authorize]
    public async Task<IActionResult> GetRequestedRidesAsync([FromQuery] GetRequestedRidesQuery query, CancellationToken cancellationToken)
    {
        var result = await _getHandler.HandleAsync(query, cancellationToken);
        return Ok(result);
    }
}
