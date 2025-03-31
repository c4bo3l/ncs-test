using System.ComponentModel.DataAnnotations;
using Infrastructure.Service.EmployeeServices.Dtos;
using MediatR;

namespace Infrastructure.Service.EmployeeServices.Requests;

public class CreateEmployeeRequest : EmployeeBaseCRURequest, IRequest<GetEmployeeDto?>
{
	
}
