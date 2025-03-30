using Microsoft.AspNetCore.Http;

namespace Infrastructure.Service.FileServices.CafeLogoServices;

public interface ICafeLogoService
{
	Task<string> UploadCafeLogoAsync(Guid cafeId, IFormFile file, CancellationToken cancellationToken);
}
