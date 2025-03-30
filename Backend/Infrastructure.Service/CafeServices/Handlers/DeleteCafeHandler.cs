using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.CafeServices.Handlers;

public class DeleteCafeHandler : IRequestHandler<DeleteCafeRequest>
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public DeleteCafeHandler(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory;
	}

	public async Task Handle(DeleteCafeRequest request, CancellationToken cancellationToken)
	{
		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
		var cafe = await context.Set<Cafe>().FindAsync([request.Id], cancellationToken);

		if (cafe is null)
		{
			return;
		}

		context.Set<Cafe>().Remove(cafe);
		await context.SaveChangesAsync(cancellationToken);
	}
}
