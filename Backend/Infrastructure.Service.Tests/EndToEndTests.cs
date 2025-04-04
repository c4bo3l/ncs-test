using Infrastructure.Database;
using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Requests;
using Infrastructure.Service.EmployeeServices.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service.Tests;

public class EndToEndTests : BaseUnitTest
{
	[Fact]
	public async Task Firstly_Created_Cafe_Has_Zero_Employee()
	{
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
		Assert.Equal(0, result.Employees);
	}

	[Fact]
	public async Task Can_Create_Employee_Without_Cafe()
	{
		var request = new CreateEmployeeRequest
		{
			Name = "Test Employee",
			Email = "test.employee@example.com",
			Phone = "81234567",
			Gender = 'M'
		};

		// Act
		var result = await Mediator.Send(request, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(request.Name, result.Name);
		Assert.Equal(request.Email, result.Email);
		Assert.Equal(request.Phone, result.Phone);
		Assert.Equal(request.Gender, result.Gender);
		Assert.False(result.CafeId.HasValue); // Employee should not be associated with any cafe
		Assert.True(string.IsNullOrEmpty(result.Cafe));
		Assert.Equal(0, result.Days_worked);
	}

	[Fact]
	public async Task Employee_Signed_To_Cafe_Show_Valid_Days_Worked()
	{
		var cafeRequest = new CreateCafeRequest
		{
			Name = "Test Cafe",
			Description = "Test Description",
			Location = "Test Location",
			Logo = "Test Logo"
		};

		var cafe = await Mediator.Send(cafeRequest, CancellationToken.None);

		var employeeRequest = new CreateEmployeeRequest
		{
			Name = "Test Employee",
			Email = "test.employee@example.com",
			Phone = "81234567",
			Gender = 'M',
			CafeId = cafe!.Id,
			StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)) // 10 days ago
		};

		var employee = await Mediator.Send(employeeRequest, CancellationToken.None);
		Assert.NotNull(employee);
		Assert.Equal(employeeRequest.Name, employee.Name);
		Assert.Equal(employeeRequest.Email, employee.Email);
		Assert.Equal(employeeRequest.Phone, employee.Phone);
		Assert.Equal(employeeRequest.Gender, employee.Gender);
		Assert.Equal(employeeRequest.CafeId, employee.CafeId);
		Assert.Equal(cafeRequest.Name, employee.Cafe);
		Assert.Equal(10, employee.Days_worked);
	}

	[Fact]
	public async Task Delete_A_Cafe_Also_Deleting_Its_Employees()
	{
		// Given
		using var scope = Container.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
		var cafeId = dbContext.Set<Employee>().Where(x => x.CafeId.HasValue).Select(x => x.CafeId!.Value).First();

		// When
		var request = new DeleteCafeRequest
		{
			Id = cafeId
		};
		await Mediator.Send(request, CancellationToken.None);

		// Then
		var cafe = await dbContext.Set<Cafe>().FindAsync(cafeId);
		Assert.Null(cafe); // Cafe should be deleted
		Assert.Empty(dbContext.Set<Employee>().Where(x => x.CafeId == cafeId)); // Employees should be deleted as well
	}
}
