using Infrastructure.Model;
using Infrastructure.Service.EmployeeServices.Dtos;

namespace Infrastructure.Service.EmployeeServices.Extensions;

public static class EmployeeExtensions
{
	public static GetEmployeeDto ToGetEmployeeDto(this Employee employee)
	{
		return new GetEmployeeDto
		{
			Id = employee.Id,
			Email = employee.Email,
			Name = employee.Name,
			Phone = employee.Phone,
			Gender = employee.Gender,
			CafeId = employee.CafeId,
			Cafe = employee.Cafe?.Name,
			Days_worked = employee.StartDate.HasValue ? DateOnly.FromDateTime(DateTime.Now).DayNumber - employee.StartDate.Value.DayNumber : 0
		};
	}
}
