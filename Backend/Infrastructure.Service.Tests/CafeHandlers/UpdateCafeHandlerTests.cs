using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Handlers;
using Infrastructure.Service.CafeServices.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service.Tests.CafeHandlers;

public class UpdateCafeHandlerTests : BaseUnitTest
{
	[Fact]
	public async Task Handle_ShouldUpdateCafe_WhenValidRequestIsProvided()
	{
		// Arrange
		using var scope = Container.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var cafeId = dbContext.Set<Cafe>().First().Id;

		var request = new UpdateCafeRequest
		{
			Id = cafeId,
			Name = "Updated Name",
			Description = "Updated Description",
			Location = "Updated Location",
			Logo = "Updated Logo"
		};

		// Act
		var result = await Mediator.Send(request, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(request.Name, result.Name);
		Assert.Equal(request.Description, result.Description);
		Assert.Equal(request.Location, result.Location);
		Assert.Equal(request.Logo, result.Logo);

		// Verify the cafe was updated in the database
		using var verificationScope = Container.CreateScope();
		var dbContextVerification = verificationScope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var updatedCafe = await dbContextVerification.Set<Cafe>().FindAsync(cafeId);
		Assert.NotNull(updatedCafe);
		Assert.Equal(request.Name, updatedCafe.Name);
		Assert.Equal(request.Description, updatedCafe.Description);
		Assert.Equal(request.Location, updatedCafe.Location);
		Assert.Equal(request.Logo, updatedCafe.Logo);
	}

	[Fact]
	public async Task Handle_ShouldThrowKeyNotFoundException_WhenCafeDoesNotExist()
	{
		// Arrange
		var request = new UpdateCafeRequest
		{
			Id = Guid.NewGuid(), // Non-existent ID
			Name = "Non-existent Cafe",
			Description = "Non-existent Description",
			Location = "Non-existent Location",
			Logo = "Non-existent Logo"
		};

		// Act & Assert
		await Assert.ThrowsAsync<KeyNotFoundException>(() => Mediator.Send(request, CancellationToken.None));
	}

	[Theory]
	[InlineData("00000000-0000-0000-0000-000000000000", "name", "Description", "Location")]
	[InlineData("00000000-0000-0000-0000-000000000001", null, "Description", "Location")]
	[InlineData("00000000-0000-0000-0000-000000000002", "Name", null, "Location")]
	[InlineData("00000000-0000-0000-0000-000000000003", "Name", "Description", null)]
	[InlineData("00000000-0000-0000-0000-000000000004", "", "Description", "Location")]
	[InlineData("00000000-0000-0000-0000-000000000005", "Name", "", "Location")]
	[InlineData("00000000-0000-0000-0000-000000000006", "Name", "Description", "")]
	public async Task Handle_ShouldThrowAnError_WhenInValidRequestIsProvided(Guid id, string? name, string? description, string? location)
	{
		// Arrange
		var request = new UpdateCafeRequest
		{
			Id = id,
			Name = name!,
			Description = description!,
			Location = location!
		};

		if (id == Guid.Empty)
		{
			await Assert.ThrowsAsync<ArgumentException>(() => Mediator.Send(request, CancellationToken.None));
			return;
		}
		await Assert.ThrowsAsync<Exception>(() => Mediator.Send(request, CancellationToken.None));
	}
	
	[Fact]
	public async Task Handle_ShouldThrowAnError_WhenNullRequestIsProvided()
	{
		Assert.Throws<ArgumentNullException>(() => new UpdateCafeHandler(null!));

		using var scope = Container.CreateScope();
		var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
		var handler = new UpdateCafeHandler(dbContextFactory);
		
		await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null!, CancellationToken.None));
	}
}
