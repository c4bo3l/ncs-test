using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.EmployeeServices.Handlers;
using Infrastructure.Service.EmployeeServices.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service.Tests.EmployeeHandlers;

public class GetEmployeeHandlerTests : BaseUnitTest
{
	[Fact]
	public async Task Handle_ShouldReturnAllEmployees_WhenNoCafeFilterIsProvided()
	{
		// Arrange
		var request = new GetEmployeesRequest { Cafe = null };

		// Act
		var result = await Mediator.Send(request, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.NotEmpty(result);
		Assert.All(result, e => Assert.NotNull(e.Name));
	}

	[Fact]
	public async Task Handle_ShouldReturnFilteredEmployees_WhenCafeFilterIsProvided()
	{
		// Arrange
		using var scope = Container.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var cafeId = dbContext.Set<Cafe>().First().Id; // Assuming there's at least one cafe in the database

		var request = new GetEmployeesRequest { Cafe = cafeId };

		// Act
		var result = await Mediator.Send(request, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.NotEmpty(result);
		Assert.All(result, e => Assert.Equal(cafeId, e.CafeId));
	}
	
	[Fact]
	public async Task Handle_ShouldThrowAnError_WhenNullRequestIsProvided()
	{
		Assert.Throws<ArgumentNullException>(() => new GetEmployeesHandler(null!));

		using var scope = Container.CreateScope();
		var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
		var handler = new GetEmployeesHandler(dbContextFactory);
		
		await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null!, CancellationToken.None));
	}
}
