namespace Acquirer.Sample.Application.Interfaces.Services;

public interface IBaseService<TEntity, TCreateModel, TUpdateModel, TResultModel, TId> 
{
    Task<Result<TResultModel>> Create(TCreateModel model);
    Task<Result<TResultModel>> Update(TUpdateModel model);
    Task<Result<TResultModel>> GetById(TId id);
    Task<Result> Delete(TId id);
}