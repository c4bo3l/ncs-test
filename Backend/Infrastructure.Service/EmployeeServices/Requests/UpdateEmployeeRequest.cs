using System.ComponentModel.DataAnnotations;
using Infrastructure.Service.EmployeeServices.Dtos;
using MediatR;

namespace Infrastructure.Service.EmployeeServices.Requests;

public class UpdateEmployeeRequest : EmployeeBaseCRURequest, IRequest<GetEmployeeDto?>
{
	[Required]
	public required string Id { get; set; }
}
