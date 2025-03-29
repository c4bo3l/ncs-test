using Infrastructure.Service.CafeServices.Dtos;
using MediatR;

namespace Infrastructure.Service.CafeServices.Requests;

public class GetCafesRequest : IRequest<GetCafeDto[]>
{
	public string? Location { get; set; } = null;
}
