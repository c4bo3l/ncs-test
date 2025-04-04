using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Service.EmployeeServices.Requests;

public abstract class EmployeeBaseCRURequest
{
	[Required]
	[MinLength(1)]
	public required string Name { get; set; }

	[Required]
	[EmailAddress]
	public required string Email { get; set; }

	[Required]
	[Length(8, 8)]
	[RegularExpression("^[89]\\d{7}")]
	public required string Phone { get; set; }

	[Required]
	[AllowedValues(['M', 'F'])]
	public required char Gender { get; set; }

	public Guid? CafeId { get; set; }
	public DateOnly? StartDate { get; set; }
}
