using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.EmployeeServices.Handlers;
using Infrastructure.Service.EmployeeServices.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service.Tests.EmployeeHandlers;

public class DeleteEmployeeHandlerTests : BaseUnitTest
{
	[Fact]
	public async Task Handle_ShouldDeleteEmployee_WhenEmployeeExists()
	{
		// Arrange
		using var scope = Container.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var employeeId = dbContext.Set<Employee>().First().Id; // Assuming there's at least one employee in the database

		var request = new DeleteEmployeeRequest { Id = employeeId };

		// Act
		await Mediator.Send(request, CancellationToken.None);

		// Assert
		using var verificationScope = Container.CreateScope();
		var dbContextVerification = verificationScope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var deletedEmployee = await dbContextVerification.Set<Employee>().FindAsync(employeeId);
		Assert.Null(deletedEmployee); // Ensure the employee was deleted
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public async Task Handle_ShouldThrowAnError_WhenInValidRequestIsProvided(string? employeeId)
	{
		// Arrange
		var request = new DeleteEmployeeRequest
		{
			Id = employeeId!
		};

		await Assert.ThrowsAsync<Exception>(() => Mediator.Send(request, CancellationToken.None));
	}
	
	[Fact]
	public async Task Handle_ShouldThrowAnError_WhenNullRequestIsProvided()
	{
		Assert.Throws<ArgumentNullException>(() => new DeleteEmployeeHandler(null!));

		using var scope = Container.CreateScope();
		var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
		var handler = new DeleteEmployeeHandler(dbContextFactory);

		await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null!, CancellationToken.None));
	}
}
