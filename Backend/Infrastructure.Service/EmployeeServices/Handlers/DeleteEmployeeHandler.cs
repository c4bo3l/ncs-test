using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.EmployeeServices.Requests;
using Infrastructure.Service.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.EmployeeServices.Handlers;

public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeRequest>
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public DeleteEmployeeHandler(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}

	public async Task Handle(DeleteEmployeeRequest request, CancellationToken cancellationToken)
	{
		if (!ObjectHelper.Validate(request, out var exception))
		{
			throw exception ?? new ArgumentNullException(nameof(request), "Request cannot be null.");
		}

		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
		var employee = await context.Set<Employee>().FindAsync([request.Id], cancellationToken);

		if (employee is null)
		{
			return;
		}

		context.Set<Employee>().Remove(employee);
		await context.SaveChangesAsync(cancellationToken);
	}
}
