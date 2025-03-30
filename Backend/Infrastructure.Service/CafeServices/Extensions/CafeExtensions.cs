using Infrastructure.Model;
using Infrastructure.Service.CafeServices.Dtos;

namespace Infrastructure.Service.CafeServices.Extensions;

public static class CafeExtensions
{
	public static GetCafeDto ToGetCafeDto(this Cafe cafe)
	{
		return new GetCafeDto
		{
			Id = cafe.Id,
			Name = cafe.Name,
			Description = cafe.Description,
			Logo = cafe.Logo,
			Location = cafe.Location,
			Employees = cafe.Employees?.Count ?? 0
		};
	}
}
