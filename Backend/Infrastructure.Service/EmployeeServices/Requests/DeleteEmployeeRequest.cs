using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Infrastructure.Service.EmployeeServices.Requests;

public class DeleteEmployeeRequest : IRequest
{
	[Required]
	[MinLength(1)]
	public required string Id { get; set; }
}
