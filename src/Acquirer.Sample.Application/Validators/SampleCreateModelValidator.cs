using Acquirer.Sample.Application.Models.Sample;
using FluentValidation;

namespace Acquirer.Sample.Domain.Validators;

public class SampleCreateModelValidator : AbstractValidator<SampleCreateModel>
{
    /* Aqui temos as validações envolvendo regras de negócio */
    public SampleCreateModelValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(300)
            .WithMessage("Nome precisa ser menor que 300 caracteres.");
    }
}
