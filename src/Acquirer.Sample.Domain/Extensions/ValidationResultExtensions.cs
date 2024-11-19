namespace Acquirer.Sample.Domain.Extensions;

public static class ValidationResultExtensions
{
    public static void AddError(this ValidationResult validation, string property, string error)
    {
        validation.Errors.Add(new ValidationFailure(property, error));
    }

    public static void AddError(this ValidationResult validation, string error)
    {
        var validationFailure = new ValidationFailure(string.Empty, error);
        validation.Errors.Add(validationFailure);
    }

    public static void AddError(this ValidationResult validation, IEnumerable<string> errors)
    {
        validation.Errors.AddRange(errors.Select(x => new ValidationFailure(string.Empty, x)));
    }

    public static void AddError(this ValidationResult validation, ValidationResult anotherValidation)
        => anotherValidation.Errors.ForEach(p => validation.AddError(p.ErrorCode, p.ErrorMessage));
}
