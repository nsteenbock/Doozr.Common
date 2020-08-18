using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Doozr.Common.Utilities.Validation
{
	public class ObjectValidator : IObjectValidator
	{
		public string GetValidationMessage<T>(T target, string propertyNameName)
		{
			var targetType = typeof(T);

			var propertyInfo = targetType.GetProperty(propertyNameName);
			if (propertyInfo != null)
			{
				var validationAttributes = propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), false).Cast<ValidationAttribute>();
				var propertyValue = propertyInfo.GetValue(target);
				foreach (var validation in validationAttributes)
				{
					var result = validation.IsValid(propertyValue);
					if (!result)
					{
						return validation.ErrorMessage;
					}
				}
			}
			return string.Empty;
		}
	}
}
