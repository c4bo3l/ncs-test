using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Handlers;
using Infrastructure.Service.CafeServices.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service.Tests.CafeHandlers;

public class CreateCafeHandlerTests : BaseUnitTest
{
	[Fact]
	public async Task Handle_ShouldCreateCafe_WhenValidRequestIsProvided()
	{
		// Arrange
		var request = new CreateCafeRequest
		{
			Name = "Test Cafe",
			Description = "Test Description",
			Location = "Test Location",
			Logo = "Test Logo"
		};

		// Act
		var result = await Mediator.Send(request, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(request.Name, result.Name);
		Assert.Equal(request.Description, result.Description);
		Assert.Equal(request.Location, result.Location);
		Assert.Equal(request.Logo, result.Logo);

		// Verify the cafe was added to the database
		using var scope = Container.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var cafeInDb = await dbContext.Set<Cafe>().FirstOrDefaultAsync(c => c.Id == result.Id);
		Assert.NotNull(cafeInDb);
	}

	[Theory]
	[InlineData(null, "Description", "Location")]
	[InlineData("Name", null, "Location")]
	[InlineData("Name", "Description", null)]
	[InlineData("", "Description", "Location")]
	[InlineData("Name", "", "Location")]
	[InlineData("Name", "Description", "")]
	public async Task Handle_ShouldThrowAnError_WhenInValidRequestIsProvided(string? name, string? description, string? location)
	{
		// Arrange
		var request = new CreateCafeRequest
		{
			Name = name!,
			Description = description!,
			Location = location!
		};

		await Assert.ThrowsAsync<Exception>(() => Mediator.Send(request, CancellationToken.None));
	}

	[Fact]
	public async Task Handle_ShouldThrowAnError_WhenNullRequestIsProvided()
	{
		Assert.Throws<ArgumentNullException>(() => new CreateCafeHandler(null!));

		using var scope = Container.CreateScope();
		var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
		var handler = new CreateCafeHandler(dbContextFactory);
		
		await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null!, CancellationToken.None));
	}
}
