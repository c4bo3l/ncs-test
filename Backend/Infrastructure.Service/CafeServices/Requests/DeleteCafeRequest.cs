using MediatR;

namespace Infrastructure.Service.CafeServices.Requests;

public class DeleteCafeRequest : IRequest
{
	public required Guid Id { get; set; }
}
