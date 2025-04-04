using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Service.Helper;

public static class ObjectHelper
{
	public static bool Validate<T>(T obj, out Exception? exception)
	{
		if (obj is null)
		{
			exception = new ArgumentNullException(nameof(obj), "Object cannot be null.");
			return false;
		}

		var validationContext = new ValidationContext(obj);
		var validationResults = new List<ValidationResult>();
		var result = Validator.TryValidateObject(obj, validationContext, validationResults, true);

		exception = validationResults.Count > 0 ?
			new Exception(string.Join(Environment.NewLine, validationResults.Select(x => x.ErrorMessage ?? ""))) : null;
		return result;
	}
}
