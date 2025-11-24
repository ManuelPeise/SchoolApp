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

        private readonly DbSet<TEntity> _table;
        public RepositoryBase(ADatabaseContext databaseContext)
        {
            _table = databaseContext.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _table.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _table
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<List<TEntity>> GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            return await _table
                .AsNoTracking()
                .Where(predicate).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _table
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int?> GetEntityId(Expression<Func<TEntity, bool>> predicate)
        {
            var entry = await _table
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);

            return entry?.Id;
        }

        public async Task AddAsync(TEntity entity, Func<TEntity, bool>? predicate)
        {
            bool exists = predicate == null ? false : _table.Any(predicate);

            if (!exists)
            {
                await _table
                    .AddAsync(entity);
            }
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            var ids = entities.Select(e => e.Id).ToList();

            var existingEntityIds = await _table
                .AsNoTracking()
                .Where(e => ids.Contains(e.Id))
                .Select(e => e.Id)
                .ToListAsync();

            await _table.AddRangeAsync(entities.Where(x => !existingEntityIds.Contains(x.Id)));
        }

        public void Update(TEntity entity)
        {
            var trackedEntry = _table
                .FirstOrDefault(e => e.Id == entity.Id);

            if (trackedEntry != null)
            {
                _table.Entry(trackedEntry).CurrentValues.SetValues(entity);
            }
            else
            {
                _table.Attach(entity);
                _table.Entry(entity).State = EntityState.Modified;
            }
        }


        public async Task RemoveAsync(int id)
        {
            var entity = await _table.FindAsync(id);

            if (entity != null)
            {
                _table.Remove(entity);
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
