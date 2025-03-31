namespace Infrastructure.Service.EmployeeServices.Dtos;

public record class GetEmployeeDto
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Email { get; set; }
	public required string Phone { get; set; }
	public required char Gender { get; set; }
	public int Days_worked { get; set; } = 0;
	public Guid? CafeId { get; set; }
	public string? Cafe { get; set; } = null;
}
