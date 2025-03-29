namespace Infrastructure.Model;

public class Cafe
{
	public required Guid Id { get; set; } = Guid.NewGuid();
	public required string Name { get; set; }
	public required string Description { get; set; }
	public string Logo { get; set; } = string.Empty;
	public required string Location { get; set; }
	public ICollection<Employee> Employees { get; set; } = [];
}
