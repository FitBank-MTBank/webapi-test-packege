using Acquirer.Sample.Domain.Enums;
using Acquirer.Sample.Domain.Extensions;
using Acquirer.Shared.Utils.Enum;

namespace Acquirer.Sample.Domain.Entities;

public class Sample : BaseEntity
{
    public Guid SampleId { get; set; }
    public string Description { get; set; }
    public SampleTypeEnum SampleType { get; set; }
    public BrandEnum Brand { get; set; }
    public DateTime? DateAt { get; set; }
    public bool Actived { get; set; }

    public ValidationResult Validate()
    {
        var validation = new ValidationResult();

        if (string.IsNullOrEmpty(Description))
        {
            validation.AddError("O valor 'Description' é inválido.");
        }

        return validation;
    }
}