namespace Infrastructure.Model;

public class Employee
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Email { get; set; }
	public required string Phone { get; set; }
	public char Gender { get; set; }

	public Guid? CafeId { get; set; }
	public Cafe? Cafe { get; set; }
	public DateOnly? StartDate { get; set; }
}
