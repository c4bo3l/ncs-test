using MediatR;

namespace Infrastructure.Service.EmployeeServices.Requests;

public class DeleteEmployeeRequest : IRequest
{
	public required string Id { get; set; }
}
