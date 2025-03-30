using Infrastructure.Service.EmployeeServices.Dtos;
using MediatR;

namespace Infrastructure.Service.EmployeeServices.Requests;

public class GetEmployeesRequest : IRequest<GetEmployeeDto[]>
{
	public string? Cafe {get; set; } = null;
}
