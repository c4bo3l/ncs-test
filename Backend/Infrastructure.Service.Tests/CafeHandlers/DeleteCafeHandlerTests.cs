using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Handlers;
using Infrastructure.Service.CafeServices.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service.Tests.CafeHandlers;

public class DeleteCafeHandlerTests : BaseUnitTest
{
	[Fact]
	public async Task Handle_ShouldDeleteCafe_WhenCafeExists()
	{
		// Arrange
		using var scope = Container.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var cafeId = dbContext.Set<Cafe>().First().Id;

		var request = new DeleteCafeRequest { Id = cafeId };

		// Act
		await Mediator.Send(request, CancellationToken.None);

		// Assert
		using var verificationScope = Container.CreateScope();
		var dbContextVerification = verificationScope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var deletedCafe = await dbContextVerification.Set<Cafe>().FindAsync(cafeId);
		Assert.Null(deletedCafe); // Ensure the cafe was deleted
	}

	[Fact]
	public async Task Handle_ShouldThrowArgumentException_WhenIdIsEmpty()
	{
		// Arrange
		var request = new DeleteCafeRequest { Id = Guid.Empty }; // Invalid ID

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(() => Mediator.Send(request, CancellationToken.None));
	}
	
	[Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenNullRequestIsProvided()
	{
		Assert.Throws<ArgumentNullException>(() => new DeleteCafeHandler(null!));

		using var scope = Container.CreateScope();
		var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
		var handler = new UpdateCafeHandler(dbContextFactory);
		
		await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null!, CancellationToken.None));
	}
}
