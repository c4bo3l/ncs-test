using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.EmployeeServices.Handlers;
using Infrastructure.Service.EmployeeServices.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service.Tests.EmployeeHandlers;

public class CreateEmployeeHandlerTests : BaseUnitTest
{
	[Fact]
	public async Task Handle_ShouldCreateEmployee_WhenValidRequestIsProvided()
	{
		// Arrange
		var request = new CreateEmployeeRequest
		{
			Name = "Test Employee",
			Email = "test.employee@example.com",
			Phone = "81234567",
			Gender = 'M',
			StartDate = DateOnly.FromDateTime(DateTime.Now)
		};

		// Act
		var result = await Mediator.Send(request, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(request.Name, result.Name);
		Assert.Equal(request.Email, result.Email);
		Assert.Equal(request.Phone, result.Phone);
		Assert.Equal(request.Gender, result.Gender);

		// Verify the employee was added to the database
		using var scope = Container.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var employeeInDb = await dbContext.Set<Employee>().FindAsync(result.Id);
		Assert.NotNull(employeeInDb);
	}

	[Fact]
	public async Task Handle_ShouldThrowArgumentException_WhenEmailAlreadyExists()
	{
		// Arrange
		using var scope = Container.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var employeeInDb = await dbContext.Set<Employee>().FirstAsync();
		var existingEmail = employeeInDb.Email.ToLower();

		var request = new CreateEmployeeRequest
		{
			Name = "New Employee",
			Email = existingEmail, // Duplicate email
			Phone = "81234567",
			Gender = 'M'
		};

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(() => Mediator.Send(request, CancellationToken.None));
	}

	[Fact]
	public async Task Handle_ShouldThrowKeyNotFoundException_WhenCafeIdIsInvalid()
	{
		// Arrange
		var invalidCafeId = Guid.NewGuid();

		var request = new CreateEmployeeRequest
		{
			Name = "Test Employee",
			Email = "test.employee@example.com",
			Phone = "81234567",
			Gender = 'M',
			CafeId = invalidCafeId // Invalid cafe ID
		};

		// Act & Assert
		await Assert.ThrowsAsync<KeyNotFoundException>(() => Mediator.Send(request, CancellationToken.None));
	}

	[Fact]
	public async Task Handle_ShouldThrowAnError_WhenNullRequestIsProvided()
	{
		Assert.Throws<ArgumentNullException>(() => new CreateEmployeeHandler(null!));

		using var scope = Container.CreateScope();
		var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
		var handler = new CreateEmployeeHandler(dbContextFactory);

		await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null!, CancellationToken.None));
	}

	[Theory]
	[InlineData(null, "asd@asd.com", "81234567", 'M')]
	[InlineData("Name", null, "81234567", 'M')]
	[InlineData("Name", "wrong email format", "81234567", 'M')]
	[InlineData("Name", "asd@asd.com", null, 'M')]
	[InlineData("Name", "asd@asd.com", "12345678", 'M')]
	[InlineData("Name", "asd@asd.com", "9234567", 'M')]
	[InlineData("Name", "asd@asd.com", "8234567", 'M')]
	[InlineData("Name", "asd@asd.com", "8234567111", 'M')]
	[InlineData("Name", "asd@asd.com", "81234567", 'T')]
	[InlineData("", "asd@asd.com", "81234567", 'M')]
	[InlineData("Name", "", "81234567", 'M')]
	[InlineData("Name", "asd@asd.com", "", 'M')]
	public async Task Handle_ShouldThrowAnError_WhenInValidRequestIsProvided(string? name, string? email, string? phone, char gender)
	{
		// Arrange
		var request = new CreateEmployeeRequest
		{
			Name = name!,
			Email = email!,
			Phone = phone!,
			Gender = gender
		};

		await Assert.ThrowsAsync<Exception>(() => Mediator.Send(request, CancellationToken.None));
	}
}
