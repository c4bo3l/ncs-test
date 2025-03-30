using Infrastructure.Service.CafeServices.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Service.CafeServices.Requests;

public class CreateCafeRequest : IRequest<GetCafeDto?>
{
	public required string Name { get; set; }
	public required string Description { get; set; }
	public IFormFile? Logo { get; set; }
	public required string Location { get; set; }
}
