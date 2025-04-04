using Infrastructure.Service.CafeServices.Handlers;
using Infrastructure.Service.CafeServices.Requests;

namespace Infrastructure.Service.Tests.CafeHandlers;

public class GetCafesHandlerTests : BaseUnitTest
{
	[Fact]
	public async Task Handle_ShouldReturnCafes_WhenLocationIsNull()
	{
		// Arrange
		var request = new GetCafesRequest { Location = null };

		// Act
		var result = await Mediator.Send(request, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.NotEmpty(result);
		Assert.All(result, cafe => Assert.NotNull(cafe.Name));
	}

	[Fact]
	public async Task Handle_ShouldReturnFilteredCafes_WhenLocationIsProvided()
	{
		// Arrange
		var request = new GetCafesRequest { Location = "Location 1" };

		// Act
		var result = await Mediator.Send(request, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.All(result, cafe => Assert.Equal("Location 1", cafe.Location));
	}

	[Fact]
	public async Task Handle_ShouldReturnEmptyCafes_WhenLocationIsNotFound()
	{
		// Arrange
		var request = new GetCafesRequest { Location = "NOT FOUND" };

		// Act
		var result = await Mediator.Send(request, CancellationToken.None);

		// Assert
		Assert.Empty(result);
	}
	
	[Fact]
	public void Handle_ShouldThrowAnError_WhenNullRequestIsProvided()
	{
		Assert.Throws<ArgumentNullException>(() => new GetCafeHandler(null!));
	}
}
