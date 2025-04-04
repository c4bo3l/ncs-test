using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Infrastructure.Service.CafeServices.Requests;

public class DeleteCafeRequest : IRequest
{
	[Required]
	public required Guid Id { get; set; }
}
