using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Dtos;
using Infrastructure.Service.CafeServices.Extensions;
using Infrastructure.Service.CafeServices.Requests;
using Infrastructure.Service.FileServices.CafeLogoServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.CafeServices.Handlers;

public class UpdateCafeHandler : IRequestHandler<UpdateCafeRequest, GetCafeDto?>
{
	private readonly IDbContextFactory<AppDbContext> dbContextFactory;
	private readonly ICafeLogoService cafeLogoService;

	public UpdateCafeHandler(IDbContextFactory<AppDbContext> dbContextFactory, ICafeLogoService cafeLogoService)
	{
		this.dbContextFactory = dbContextFactory;
		this.cafeLogoService = cafeLogoService;
	}

	public async Task<GetCafeDto?> Handle(UpdateCafeRequest request, CancellationToken cancellationToken)
	{
		using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

		var cafe = await context.Set<Cafe>().FindAsync([request.Id], cancellationToken)
			?? throw new KeyNotFoundException($"Cafe with Id {request.Id} not found");

		context.Entry(cafe).CurrentValues.SetValues(request);

		if (request.LogoFile is not null)
		{
			cafe.Logo = await cafeLogoService.UploadCafeLogoAsync(cafe.Id, request.LogoFile, cancellationToken);
		}
		
		if (await context.SaveChangesAsync(cancellationToken) != 1)
		{
			return null;
		}

		return cafe.ToGetCafeDto();
	}
}
