using Infrastructure.Service.CafeServices.Dtos;
using MediatR;

namespace Infrastructure.Service.CafeServices.Requests;

public class CreateCafeRequest : CafeBaseCRURequest, IRequest<GetCafeDto?>
{
	
}
