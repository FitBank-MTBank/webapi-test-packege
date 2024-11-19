namespace Acquirer.Sample.Api.Extensions;

public static class ValidationExtensions
{
    public static IEnumerable<string> ListErrors(this ValidationResult validationResult)
        => validationResult.Errors.Select(p => $"{(!string.IsNullOrEmpty(p.PropertyName) ? $"'{p.PropertyName}' " : "")}{p.ErrorMessage}");
}