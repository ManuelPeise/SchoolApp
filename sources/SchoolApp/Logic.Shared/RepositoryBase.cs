using Data.Entities;
using Data.Shared;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Logic.Shared
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : AEntityBase
    {
        private bool _disposed;
        private readonly ADatabaseContext _dbContext;

        public RepositoryBase(ADatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<List<TEntity>> GetAll(bool asNoTracking = false)
        {
            var query = asNoTracking
                 ? _dbContext.Set<TEntity>().AsNoTracking()
                 : _dbContext.Set<TEntity>();

            return await query.ToListAsync();
        }

        public async Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false, params Expression<Func<TEntity, object>>[]? includeExpressions)
        {
            var query = asNoTracking
                ? _dbContext.Set<TEntity>().AsNoTracking()
                : _dbContext.Set<TEntity>();

            query = query.Where(predicate);

            if (includeExpressions != null)
            {
                foreach (var expression in includeExpressions)
                {
                   query = query.Include(expression);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetBy(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false, params Expression<Func<TEntity, object>>[]? includeExpressions)
        {
            var query = asNoTracking
                ? _dbContext.Set<TEntity>().AsNoTracking()
                : _dbContext.Set<TEntity>();

            query = query.Where(predicate);

            if (includeExpressions != null)
            {
                foreach (var expression in includeExpressions)
                {
                   query = query.Include(expression);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id, bool asNoTracking = false, params Expression<Func<TEntity, object>>[]? includeExpressions)
        {
            var query = asNoTracking
                ? _dbContext.Set<TEntity>().AsNoTracking()
                : _dbContext.Set<TEntity>();

            query = query.Where(x => x.Id == id);

            if (includeExpressions != null)
            {
                foreach (var expression in includeExpressions)
                {
                    query = query.Include(expression);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int?> GetEntityId(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false, params Expression<Func<TEntity, object>>[]? includeExpressions)
        {
            var query = asNoTracking
                ? _dbContext.Set<TEntity>().AsNoTracking()
                : _dbContext.Set<TEntity>();

            query = query.Where(predicate);

            if (includeExpressions != null)
            {
                foreach (var expression in includeExpressions)
                {
                    query = query.Include(expression);
                }
            }

            var entity = await query.FirstOrDefaultAsync();

            return entity?.Id;
        }

        public async Task AddAsync(TEntity entity, Func<TEntity, bool>? predicate, bool asNoTracking = false)
        {
            var query = asNoTracking
               ? _dbContext.Set<TEntity>().AsNoTracking()
               : _dbContext.Set<TEntity>();

            bool exists = predicate == null ? false : query.Any(predicate);

            if (!exists)
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
            }
        }

        public async Task AddRangeAsync(List<TEntity> entities, bool asNoTracking = false)
        {
            var ids = entities.Select(e => e.Id).ToList();

            var query = asNoTracking
               ? _dbContext.Set<TEntity>().AsNoTracking()
               : _dbContext.Set<TEntity>();

            var existingEntityIds = await query
                .Where(e => ids.Contains(e.Id))
                .Select(e => e.Id)
                .ToListAsync();

            await _dbContext.Set<TEntity>().AddRangeAsync(entities.Where(x => !existingEntityIds.Contains(x.Id)));
        }

        public void Update(TEntity entity, bool asNoTracking = false)
        {
            var query = asNoTracking
               ? _dbContext.Set<TEntity>().AsNoTracking()
               : _dbContext.Set<TEntity>();

            var trackedEntry = query
                .FirstOrDefault(e => e.Id == entity.Id);

            if (trackedEntry != null)
            {
                _dbContext.Set<TEntity>().Entry(trackedEntry).CurrentValues.SetValues(entity);
            }
            else
            {
                _dbContext.Set<TEntity>().Attach(entity);
                _dbContext.Set<TEntity>().Entry(entity).State = EntityState.Modified;
            }
        }


        public async Task RemoveAsync(int id, bool asNoTracking = false)
        {
            var query = asNoTracking
              ? _dbContext.Set<TEntity>().AsNoTracking()
              : _dbContext.Set<TEntity>();

            var entity = await query.FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {

                _disposed = true;
            }
        }

    }
}
