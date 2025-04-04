using Infrastructure.Database;
using Infrastructure.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service.Tests;

public abstract class BaseUnitTest
{
	private readonly IServiceCollection _serviceCollection;
	protected IServiceProvider Container { get; }

	protected IMediator Mediator => Container.GetRequiredService<IMediator>();
	protected Guid DbId { get; private set; }

	protected BaseUnitTest()
	{
		DbId = Guid.NewGuid();
		_serviceCollection = new ServiceCollection();
		RegisterDependencies();
		Container = _serviceCollection.BuildServiceProvider();
		SeedingData();
	}

	protected virtual void SeedingData()
	{
		using var dbContext = Container.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		dbContext.Database.EnsureCreated();
	}

	protected virtual void RegisterDependencies()
	{
		_serviceCollection.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(typeof(RegisterHelper).Assembly);
		});
		_serviceCollection.AddScoped<IDbContextFactory<AppDbContext>>((_) =>
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(DbId.ToString())
				.UseSeeding((context, _) =>
				{
					Guid[] cafeIds = [
					Guid.Parse("1b5c15a6-54e2-4fae-9cc7-6a6848e70e72"),
					Guid.Parse("df06a1d5-a3bc-4a7d-8c89-cf981d36cd19"),
					Guid.Parse("08e81e98-5c0f-4f3b-b0f5-7db5f1b7f3cf"),
					Guid.Parse("5ce0e5cb-e173-40a3-b8e6-94652dc26c51"),
					Guid.Parse("27121da9-d9a6-4bab-8151-639183614756")
				];
					var hasMissingData = false;

					for (var i = 0; i < cafeIds.Length; i++)
					{
						if (!context.Set<Cafe>().Any(x => x.Id == cafeIds[0]))
						{
							context.Set<Cafe>().Add(new Cafe
							{
								Id = cafeIds[i],
								Name = $"Cafe {i + 1}",
								Description = $"Desc Cafe {i + 1}",
								Location = $"Location {(i % 2) + 1}",
							});

							hasMissingData = true;
						}
					}

					foreach (var i in Enumerable.Range(0, 20))
					{
						var employeeId = $"UI{i:0000000}";
						if (!context.Set<Employee>().Any(x => x.Id == employeeId))
						{
							context.Set<Employee>().Add(new Employee
							{
								Id = employeeId,
								Name = $"Employee {i}",
								Email = $"employee{i}@fakeemail.com",
								Phone = $"{(i % 2 == 0 ? "8" : "9")}1234567",
								Gender = i % 2 == 0 ? 'F' : 'M',
								CafeId = cafeIds[i % cafeIds.Length],
								StartDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(i)))
							});

							hasMissingData = true;
						}
					}

					if (hasMissingData)
					{
						context.SaveChanges();
					}
				})
				.Options;

			return new PooledDbContextFactory<AppDbContext>(options);
		});
	}
}
