using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Acquirer.Sample.Domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity, TId> where TEntity : class
{
    Task Create(TEntity entity);
    Task CreateRange(IEnumerable<TEntity> entities);
    Task Update(TEntity entity);
    Task UpdateRange(IEnumerable<TEntity> entities);
    Task<TEntity> GetById(TId id);
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
    Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> GetAll();
    Task Delete(TEntity entity);
    Task DeleteRange(IEnumerable<TEntity> entities);
}
