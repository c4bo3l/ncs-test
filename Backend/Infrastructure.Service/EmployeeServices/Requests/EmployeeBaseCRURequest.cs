using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Service.EmployeeServices.Requests;

public abstract class EmployeeBaseCRURequest
{
	public required string Name { get; set; }

	[EmailAddress]
	public required string Email { get; set; }

	[Length(8, 8)]
	[RegularExpression("^[89]\\d{7}")]
	public required string Phone { get; set; }

	[AllowedValues(['M', 'F'])]
	public required char Gender { get; set; }

	public Guid? CafeId { get; set; }
	public DateOnly? StartDate { get; set; }
}
