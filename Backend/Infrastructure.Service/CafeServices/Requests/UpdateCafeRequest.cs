using Infrastructure.Service.CafeServices.Dtos;
using MediatR;

namespace Infrastructure.Service.CafeServices.Requests;

public class UpdateCafeRequest : CafeBaseCRURequest, IRequest<GetCafeDto?>
{
	public required Guid Id { get; set; }
}
