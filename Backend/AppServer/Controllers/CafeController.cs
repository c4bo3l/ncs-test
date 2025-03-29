using Infrastructure.Service.CafeServices.Dtos;
using Infrastructure.Service.CafeServices.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppServer.Controllers;
[Route("api")]
[ApiController]
public class CafeController : ControllerBase
{
    private readonly IMediator _mediator;
    public CafeController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("cafes")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCafeDto[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetCafes([FromQuery] GetCafesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var cafes = _mediator.Send(request, cancellationToken);
            return Ok(cafes.Result);   
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
