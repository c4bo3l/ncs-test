using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppServer.Extensions;

public static class ConfigureDatabaseExtension
{
	public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextPool<AppDbContext>(options =>
		{
			var connectionString = configuration.GetConnectionString("AppContext") ??
									  throw new Exception("Connection string not found");
			options.UseNpgsql(connectionString);
		});
	}
}
