using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.EmployeeServices.Dtos;
using Infrastructure.Service.EmployeeServices.Extensions;
using Infrastructure.Service.EmployeeServices.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.EmployeeServices.Handlers;

public class GetEmployeesHandler : IRequestHandler<GetEmployeesRequest, GetEmployeeDto[]>
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public GetEmployeesHandler(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}

	public async Task<GetEmployeeDto[]> Handle(GetEmployeesRequest request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request, nameof(request));
		
		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

		var employees = context.Set<Employee>()
			.Include(c => c.Cafe)
			.AsNoTracking()
			.Where(c => !request.Cafe.HasValue || c.CafeId == request.Cafe.Value)
			.OrderBy(x => x.StartDate)
			.AsAsyncEnumerable();

		var result = new List<GetEmployeeDto>();

		await foreach (var employee in employees.WithCancellation(cancellationToken))
		{
			result.Add(employee.ToGetEmployeeDto());
		}

		return [.. result];
	}
}
