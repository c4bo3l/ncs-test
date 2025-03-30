namespace Infrastructure.Service.EmployeeServices.Dtos;

public record class GetEmployeeDto
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Email { get; set; }
	public required string Phone { get; set; }
	public int Days_worked { get; set; } = 0;
	public string? Cafe { get; set; } = null;
}
