namespace Infrastructure.Service.CafeServices.Requests;

public abstract class CafeBaseCRURequest
{
	public required string Name { get; set; }
	public required string Description { get; set; }

	public string? Logo { get; set; }

	public required string Location { get; set; }
}
