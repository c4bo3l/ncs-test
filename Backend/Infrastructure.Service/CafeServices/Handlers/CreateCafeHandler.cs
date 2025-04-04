using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Dtos;
using Infrastructure.Service.CafeServices.Extensions;
using Infrastructure.Service.CafeServices.Requests;
using Infrastructure.Service.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.CafeServices.Handlers;

public class CreateCafeHandler : IRequestHandler<CreateCafeRequest, GetCafeDto?>
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;

	public CreateCafeHandler(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}

	public async Task<GetCafeDto?> Handle(CreateCafeRequest request, CancellationToken cancellationToken)
	{
		if (!ObjectHelper.Validate(request, out var exception))
		{
			throw exception ?? new ArgumentNullException(nameof(request), "Request cannot be null.");
		}

		var cafe = new Cafe
		{
			Id = Guid.NewGuid(),
			Name = request.Name,
			Description = request.Description,
			Location = request.Location,
			Logo = request.Logo
		};

		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
		await context.Set<Cafe>().AddAsync(cafe, cancellationToken);

		if (await context.SaveChangesAsync(cancellationToken) != 1)
		{
			return null;
		}

		return cafe.ToGetCafeDto();
	}
}
