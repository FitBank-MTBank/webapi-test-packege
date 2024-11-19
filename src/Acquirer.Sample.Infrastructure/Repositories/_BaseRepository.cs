using Acquirer.Sample.Domain.Interfaces.Repositories;
using Acquirer.Sample.Infrastructure.Persistence;
using System.Linq.Expressions;
using System.Threading;

namespace Acquirer.Sample.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : class
{
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly SampleDbContext _dbContext;

    protected BaseRepository(SampleDbContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task Create(TEntity entity)
    {
        await _dbSet.AddAsync(entity).ConfigureAwait(false);
        await SaveChanges();
    }

    public async Task CreateRange(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await SaveChanges();
    }

    public async Task Update(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await SaveChanges();
    }

    public async Task UpdateRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        await SaveChanges();
    }

    public async Task<TEntity> GetById(TId id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbContext.Set<TEntity>().Where(predicate);
    }

    public async Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>();
    }

    public async Task Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
        await SaveChanges();
    }

    public async Task DeleteRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
        await SaveChanges();
    }

    private async Task<int> SaveChanges(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}