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
    public async Task<IActionResult> GetCafes([FromQuery] GetCafesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var cafes = await _mediator.Send(request, cancellationToken);
            return Ok(cafes);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("cafe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCafeDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddCafe([FromForm] CreateCafeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var cafe = await _mediator.Send(request, cancellationToken);
            if (cafe is null)
            {
                return Conflict();
            }
            return Ok(cafe);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("cafe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCafeDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCafe([FromForm] UpdateCafeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var cafe = await _mediator.Send(request, cancellationToken);
            if (cafe is null)
            {
                return Conflict();
            }
            return Ok(cafe);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("cafe")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCafe([FromBody] DeleteCafeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(request, cancellationToken);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
