using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Service.CafeServices.Requests;

public abstract class CafeBaseCRURequest
{
	[Required]
	[MinLength(1)]
	public required string Name { get; set; }

	[Required]
	[MinLength(1)]
	public required string Description { get; set; }

	public string? Logo { get; set; }

	[Required]
	[MinLength(1)]
	public required string Location { get; set; }
}
