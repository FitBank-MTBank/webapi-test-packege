using Acquirer.Sample.Application.Models.Sample;
using FluentValidation;

namespace Acquirer.Sample.Domain.Validators;

public class SampleUpdateModelValidator : AbstractValidator<SampleUpdateModel>
{
    /* Aqui temos as validações envolvendo regras de negócio */
    public SampleUpdateModelValidator()
    {
        RuleFor(x => x.SampleId)
            .Empty()
            .WithMessage("SampleId deve ser preenchida.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(300)
            .WithMessage("Nome precisa ser menor que 300 caracteres.");
    }
}