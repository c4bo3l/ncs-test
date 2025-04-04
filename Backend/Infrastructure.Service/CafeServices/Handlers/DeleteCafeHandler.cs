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
		this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}

	public async Task Handle(DeleteCafeRequest request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request, nameof(request));

		if (request.Id == Guid.Empty)
		{
			throw new ArgumentException("Cafe Id cannot be empty.", nameof(request.Id));
		}

		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
		var cafe = await context.Set<Cafe>().FindAsync([request.Id], cancellationToken);

		if (cafe is null)
		{
			return;
		}

		context.Set<Cafe>().Remove(cafe);

		// Remove employees associated with the cafe
		var employees = await context.Set<Employee>()
			.Where(e => e.CafeId == request.Id)
			.ToListAsync(cancellationToken);
		if (employees.Count > 0)
		{
			context.Set<Employee>().RemoveRange(employees);
		}

		await context.SaveChangesAsync(cancellationToken);
	}
}
