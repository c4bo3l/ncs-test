using Infrastructure.Service.EmployeeServices.Dtos;
using MediatR;

namespace Infrastructure.Service.EmployeeServices.Requests;

public class UpdateEmployeeRequest : EmployeeBaseCRURequest, IRequest<GetEmployeeDto?>
{
	public required string Id { get; set; }
}
