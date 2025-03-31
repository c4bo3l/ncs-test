using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Dtos;
using Infrastructure.Service.CafeServices.Extensions;
using Infrastructure.Service.CafeServices.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.CafeServices.Handlers;

public class UpdateCafeHandler : IRequestHandler<UpdateCafeRequest, GetCafeDto?>
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public UpdateCafeHandler(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory;
	}

	public async Task<GetCafeDto?> Handle(UpdateCafeRequest request, CancellationToken cancellationToken)
	{
		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

		var cafe = await context.Set<Cafe>().FindAsync([request.Id], cancellationToken)
			?? throw new KeyNotFoundException($"Cafe with Id {request.Id} not found");

		context.Entry(cafe).CurrentValues.SetValues(request);

		if (await context.SaveChangesAsync(cancellationToken) != 1)
		{
			return null;
		}

		return cafe.ToGetCafeDto();
	}
}
