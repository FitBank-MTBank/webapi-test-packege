using Acquirer.Sample.Domain.Validators;
using FluentValidation;
using System.Text.Json.Serialization;

namespace Acquirer.Sample.Application.Models.Sample;

public class SampleModelBase
{
    public string Description { get; set; }
    public string Type { get; set; }
    public DateTime DateAt { get; set; }
}

public class SampleCreateModel : SampleModelBase 
{ 
    public Result<SampleResultModel> Validate()
    {
        var validation = new SampleCreateModelValidator();

        var validationResult = validation.Validate(this);

        if (!validationResult.IsValid)
            return Result.Failure<SampleResultModel>(validationResult, default, Error.ValidationError);

        return Result.Success<SampleResultModel>(default);
    }
}

public class SampleUpdateModel : SampleModelBase
{
    [JsonIgnore]
    public Guid SampleId { get; set; }
    public bool Active { get; set; }

    public Result<SampleResultModel> Validate()
    {
        var validation = new SampleUpdateModelValidator();

        var validationResult = validation.Validate(this);

        if (!validationResult.IsValid)
            return Result.Failure<SampleResultModel>(validationResult, default, Error.ValidationError);

        return Result.Success<SampleResultModel>(default);
    }
}

public class SampleResultModel : SampleModelBase
{
    public Guid SampleId { get; set; }
    public bool Active { get; set; }
}