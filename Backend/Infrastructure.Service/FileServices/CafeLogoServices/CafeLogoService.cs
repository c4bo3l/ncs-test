using Microsoft.AspNetCore.Http;

namespace Infrastructure.Service.FileServices.CafeLogoServices;

public class CafeLogoService : ICafeLogoService
{
	private readonly string CafeLogoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cafe-logo");

	public async Task<string> UploadCafeLogoAsync(Guid cafeId, IFormFile file, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(file, nameof(file));

		if (file.Length == 0)
		{
			throw new ArgumentException("File can't be empty", nameof(file));
		}

		if (!Directory.Exists(CafeLogoPath))
		{
			Directory.CreateDirectory(CafeLogoPath);
		}

		var ext = GetFileExtension(file.FileName);
		var filename = string.Format("{0}{1}", cafeId, !string.IsNullOrEmpty(ext) ? $".{ext}" : string.Empty);
		var filePath = Path.Combine(CafeLogoPath, filename);
		using var stream = File.Create(filePath);
		await file.CopyToAsync(stream, cancellationToken);
		return filename;
	}

	private string? GetFileExtension(string filename)
	{
		var splits = filename.Split('.');
		return splits.Length < 1 ? null : splits[splits.Length - 1];
	}
}
