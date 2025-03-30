using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Dtos;
using Infrastructure.Service.CafeServices.Extensions;
using Infrastructure.Service.CafeServices.Requests;
using Infrastructure.Service.FileServices.CafeLogoServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.CafeServices.Handlers;

public class CreateCafeHandler : IRequestHandler<CreateCafeRequest, GetCafeDto?>
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;
	private readonly ICafeLogoService cafeLogoService;

	public CreateCafeHandler(IDbContextFactory<AppDbContext> dbContextFactory, ICafeLogoService cafeLogoService)
	{
		this.dbContextFactory = dbContextFactory;
		this.cafeLogoService = cafeLogoService;
	}

	public async Task<GetCafeDto?> Handle(CreateCafeRequest request, CancellationToken cancellationToken)
	{
		var cafe = new Cafe
		{
			Id = Guid.NewGuid(),
			Name = request.Name,
			Description = request.Description,
			Location = request.Location
		};

		if (request.Logo is not null)
		{
			cafe.Logo = await cafeLogoService.UploadCafeLogoAsync(cafe.Id, request.Logo, cancellationToken);
		}

		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
		await context.Set<Cafe>().AddAsync(cafe, cancellationToken);

		if (await context.SaveChangesAsync(cancellationToken) != 1)
		{
			return null;
		}

		return cafe.ToGetCafeDto();
	}
}
