using Acquirer.Sample.Application.Interfaces.Services;
using Acquirer.Sample.Application.Models.Sample;
using Acquirer.Sample.Domain.Interfaces.Repositories;
using Mapster;

namespace Acquirer.Sample.Application.Services;

public class SampleService(ISampleRepository sampleRepository) : ISampleService
{
    public async Task<Result<SampleResultModel>> Create(SampleCreateModel model)
    {
        var validationResult = new ValidationResult();

        var validationModel = model.Validate();
        if (validationModel.IsFailure)
        {
            return validationModel;
        }

        var sampleEntity = model.Adapt<Domain.Entities.Sample>();
        sampleEntity.Actived = true;

        if (!validationResult.IsValid)
            return Result.Failure<SampleResultModel>(validationResult, default, Error.ValidationError);

        await sampleRepository.Create(sampleEntity);

        //Retorna a model de consulta
        var modelResult = sampleEntity.Adapt<SampleResultModel>();

        modelResult.SampleId = sampleEntity.SampleId;

        return Result.Success(validationResult, modelResult);
    }

    public async Task<Result<SampleResultModel>> Update(SampleUpdateModel model)
    {
        var validationResult = new ValidationResult();

        var validationModel = model.Validate();
        if (validationModel.IsFailure)
        {
            return validationModel;
        }

        var sampleEntity = await sampleRepository.GetById(model.SampleId);

        if (sampleEntity is null)
        {
            validationResult.AddError("Registro não encontrado.");
            return Result.Failure<SampleResultModel>(validationResult, default, Error.ValidationError);
        }

        sampleEntity.Description = model.Description;
        sampleEntity.SampleType = (Domain.Enums.SampleTypeEnum)Convert.ToInt32(model.Type);
        sampleEntity.DateAt = model.DateAt;
        sampleEntity.Actived = true;

        validationResult = sampleEntity.Validate();
        if (!validationResult.IsValid)
            return Result.Failure<SampleResultModel>(validationResult, default, Error.ValidationError);

        await sampleRepository.Update(sampleEntity);

        var modelResult = sampleEntity.Adapt<SampleResultModel>();

        return Result.Success(validationResult, modelResult);
    }

    public async Task<Result<SampleResultModel>> GetById(Guid sampleId)
    {
        var validationResult = new ValidationResult();

        var entity = await sampleRepository.GetById(sampleId);

        if (entity is null)
        {
            validationResult.AddError("Registro não encontrado.");
            return Result.Failure<SampleResultModel>(validationResult, default, Error.ValidationError);
        }

        var model = entity.Adapt<SampleResultModel>();

        return Result.Success(validationResult, model);
    }

    public async Task<Result> Delete(Guid sampleId)
    {
        var validationResult = new ValidationResult();

        var sampleEntity = await sampleRepository.GetById(sampleId);

        if (sampleEntity is null)
        {
            validationResult.AddError("Registro não encontrado.");
            return Result.Failure<SampleResultModel>(validationResult, default, Error.ValidationError);
        }

        sampleEntity.Excluded = true;
        await sampleRepository.Update(sampleEntity);

        return Result.Success(validationResult);
    }
}