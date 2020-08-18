namespace Doozr.Common.Utilities.Validation
{
	public interface IObjectValidator
	{
		string GetValidationMessage<T>(T target, string propertyNameName);
	}
}
