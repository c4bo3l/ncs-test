using System;
using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.EmployeeServices.Dtos;
using Infrastructure.Service.EmployeeServices.Extensions;
using Infrastructure.Service.EmployeeServices.Requests;
using Infrastructure.Service.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.EmployeeServices.Handlers;

public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeRequest, GetEmployeeDto?>
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public UpdateEmployeeHandler(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}

	public async Task<GetEmployeeDto?> Handle(UpdateEmployeeRequest request, CancellationToken cancellationToken)
	{
		if (!ObjectHelper.Validate(request, out var exception))
		{
			throw exception ?? new ArgumentNullException(nameof(request), "Request cannot be null.");
		}

		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

		if (await context.Set<Employee>().AnyAsync(x => x.Email.ToLower() == request.Email.ToLower() && x.Id != request.Id, cancellationToken))
		{
			throw new ArgumentException("This email has been existed", nameof(CreateEmployeeRequest.Email));
		}

		var employee = await context.Set<Employee>()
			.Include(x => x.Cafe)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
			?? throw new KeyNotFoundException($"Employee with Id {request.Id} not found");

		if (request.CafeId.HasValue && employee.CafeId != request.CafeId.Value)
		{
			var cafe = await context.Set<Cafe>().FindAsync([request.CafeId.Value], cancellationToken)
				?? throw new KeyNotFoundException($"Cafe Id {request.CafeId.Value} not found");
			employee.CafeId = cafe.Id;
			employee.Cafe = cafe;
			employee.StartDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.Now);
		}
		else if (!request.CafeId.HasValue && employee.CafeId.HasValue)
		{
			employee.Cafe = null;
			employee.StartDate = null;
		}

		context.Entry(employee).CurrentValues.SetValues(request);
		await context.SaveChangesAsync(cancellationToken);

		return employee.ToGetEmployeeDto();
	}
}
