using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Service.CafeServices.Requests;

public abstract class CafeBaseCRURequest
{
	public required string Name { get; set; }
	public required string Description { get; set; }

	public IFormFile? LogoFile { get; set; }

	public required string Location { get; set; }
}
