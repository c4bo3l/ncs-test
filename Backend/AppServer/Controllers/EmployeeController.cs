using Infrastructure.Service.EmployeeServices.Dtos;
using Infrastructure.Service.EmployeeServices.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppServer.Controllers;

[Route("api")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IMediator mediator;

    public EmployeeController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("employees")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEmployeeDto[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEmployees([FromQuery] GetEmployeesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("employee")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEmployeeDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(request, cancellationToken);

            if (result is null)
            {
                return Conflict();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("employee")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEmployeeDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(request, cancellationToken);

            if (result is null)
            {
                return Conflict();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("employee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteEmployee([FromBody] DeleteEmployeeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await mediator.Send(request, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
