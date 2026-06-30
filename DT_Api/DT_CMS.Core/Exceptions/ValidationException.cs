namespace DT_CMS.Core.Exceptions;

public class ValidationException(IEnumerable<string> errors) : Exception("One or more validation failures occurred.")
{
    public IEnumerable<string> Errors { get; } = errors;
}
