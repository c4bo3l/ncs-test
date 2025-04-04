using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.EmployeeServices.Handlers;
using Infrastructure.Service.EmployeeServices.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service.Tests.EmployeeHandlers;

public class UpdateEmployeeHandlerTests : BaseUnitTest
{
	[Fact]
	public async Task Handle_ShouldUpdateEmployee_WhenValidRequestIsProvided()
	{
		// Arrange
		using var scope = Container.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var employeeId = dbContext.Set<Employee>().First().Id; // Assuming there's at least one employee in the database

		var request = new UpdateEmployeeRequest
		{
			Id = employeeId,
			Name = "Updated Name",
			Email = "updated.email@example.com",
			Phone = "97654321",
			Gender = 'F'
		};

		// Act
		var result = await Mediator.Send(request, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(request.Name, result.Name);
		Assert.Equal(request.Email, result.Email);
		Assert.Equal(request.Phone, result.Phone);
		Assert.Equal(request.Gender, result.Gender);

		// Verify the employee was updated in the database
		using var verificationScope = Container.CreateScope();
		var dbContextVerification = verificationScope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var updatedEmployee = await dbContextVerification.Set<Employee>().FindAsync(employeeId);
		Assert.NotNull(updatedEmployee);
		Assert.Equal(request.Name, updatedEmployee.Name);
		Assert.Equal(request.Email, updatedEmployee.Email);
	}

	[Fact]
	public async Task Handle_ShouldThrowArgumentException_WhenEmailAlreadyExists()
	{
		// Arrange
		using var scope = Container.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var employee1 = dbContext.Set<Employee>().First(); // Assuming there's at least one employee in the database
		var employee2 = dbContext.Set<Employee>().Last(); // Assuming there's at least one employee in the database


		var request = new UpdateEmployeeRequest
		{
			Id = employee1.Id,
			Name = "Updated Name",
			Phone = "97654321",
			Gender = 'F',
			Email = employee2.Email // Duplicate email
		};

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(() => Mediator.Send(request, CancellationToken.None));
	}

	[Fact]
	public async Task Handle_ShouldThrowKeyNotFoundException_WhenEmployeeDoesNotExist()
	{
		// Arrange
		var request = new UpdateEmployeeRequest
		{
			Id = "NonExistentId",
			Name = "Updated Name",
			Email = "updated.email@example.com",
			Phone = "97654321",
			Gender = 'F'
		};

		// Act & Assert
		await Assert.ThrowsAsync<KeyNotFoundException>(() => Mediator.Send(request, CancellationToken.None));
	}

	[Fact]
	public async Task Handle_ShouldThrowKeyNotFoundException_WhenCafeIdIsInvalid()
	{
		// Arrange
		using var scope = Container.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var employeeId = dbContext.Set<Employee>().First().Id; // Assuming there's at least one employee in the database

		var request = new UpdateEmployeeRequest
		{
			Id = employeeId,
			Name = "Updated Name",
			Email = "updated.email@example.com",
			Phone = "97654321",
			Gender = 'F',
			CafeId = Guid.NewGuid() // Invalid cafe ID
		};

		// Act & Assert
		await Assert.ThrowsAsync<KeyNotFoundException>(() => Mediator.Send(request, CancellationToken.None));
	}
	
	[Fact]
	public async Task Handle_ShouldThrowAnError_WhenNullRequestIsProvided()
	{
		Assert.Throws<ArgumentNullException>(() => new UpdateEmployeeHandler(null!));

		using var scope = Container.CreateScope();
		var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
		var handler = new UpdateEmployeeHandler(dbContextFactory);

		await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null!, CancellationToken.None));
	}
}
