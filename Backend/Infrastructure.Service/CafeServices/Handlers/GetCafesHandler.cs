using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Dtos;
using Infrastructure.Service.CafeServices.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.CafeServices.Handlers;

public class GetCafeHandler : IRequestHandler<GetCafesRequest, GetCafeDto[]>
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public GetCafeHandler(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}

	public async Task<GetCafeDto[]> Handle(GetCafesRequest request, CancellationToken cancellationToken)
	{
		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

		var location = request.Location?.ToLowerInvariant()?.Trim();

		var cafes = await context.Set<Cafe>()
			.Include(c => c.Employees)
			.AsNoTracking()
			.Where(c => string.IsNullOrEmpty(location) || c.Location.ToLower() == location)
			.Select(c => new GetCafeDto
			{
				Id = c.Id,
				Name = c.Name,
				Description = c.Description,
				Logo = c.Logo,
				Location = c.Location,
				Employees = c.Employees.Count()
			})
			.OrderByDescending(x => x.Employees).ThenBy(x => x.Name)
			.ToArrayAsync(cancellationToken);

		return cafes;
	}
}
