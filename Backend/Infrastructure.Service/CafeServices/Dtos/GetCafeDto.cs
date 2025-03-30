namespace Infrastructure.Service.CafeServices.Dtos;

public record class GetCafeDto
{
	public Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
	public string? Logo { get; set; } = null;
	public required string Location { get; set; }
	public int Employees { get; set; } = 0;
}
