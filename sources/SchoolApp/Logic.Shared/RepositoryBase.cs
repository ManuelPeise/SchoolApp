using Data.Context;
using Data.Entities;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Logic.Shared
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : AEntityBase
    {
        protected readonly AppDbContext _dbContext;

        private bool _disposed;

        public RepositoryBase(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TEntity>> GetAll() => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<TEntity>> GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
            => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task<int?> GetEntityId(Expression<Func<TEntity, bool>> predicate)
        {
            var entry = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);

            return entry?.Id;                        
        }

        public async Task AddAsync(TEntity entity, Func<TEntity, bool>? predicate)
        {
            bool exists = predicate == null ? false : _dbContext.Set<TEntity>().Any(predicate);
            
            if (!exists)
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public void Update(TEntity entity)
            => _dbContext.Set<TEntity>().Update(entity);

        public async Task RemoveAsync(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _dbContext.Dispose();
                _disposed = true;
            }
        }

    }
}
