using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.EmployeeServices.Dtos;
using Infrastructure.Service.EmployeeServices.Extensions;
using Infrastructure.Service.EmployeeServices.Requests;
using Infrastructure.Service.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NanoidDotNet;

namespace Infrastructure.Service.EmployeeServices.Handlers;

public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeRequest, GetEmployeeDto?>
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public CreateEmployeeHandler(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}

	public async Task<GetEmployeeDto?> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken)
	{
		if (!ObjectHelper.Validate(request, out var exception))
		{
			throw exception ?? new ArgumentNullException(nameof(request), "Request cannot be null.");
		}
		
		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

		if (await context.Set<Employee>().AnyAsync(x => x.Email.ToLower() == request.Email.ToLower(), cancellationToken))
		{
			throw new ArgumentException("This email has been existed", nameof(CreateEmployeeRequest.Email));
		}

		var employee = new Employee
		{
			Id = string.Format("UI{0}", Nanoid.Generate(Nanoid.Alphabets.LettersAndDigits, 7)),
			Name = request.Name,
			Email = request.Email,
			Phone = request.Phone,
			Gender = request.Gender
		};

		if (request.CafeId.HasValue)
		{
			var cafe = await context.Set<Cafe>().FindAsync([request.CafeId.Value], cancellationToken)
				?? throw new KeyNotFoundException($"Cafe Id {request.CafeId.Value} not found");
			employee.CafeId = cafe.Id;
			employee.Cafe = cafe;
			employee.StartDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.Now);
		}

		await context.Set<Employee>().AddAsync(employee, cancellationToken);
		if (await context.SaveChangesAsync(cancellationToken) != 1)
		{
			return null;
		}

		return employee.ToGetEmployeeDto();
	}
}
