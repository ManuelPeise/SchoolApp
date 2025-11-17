using Data.Context;
using Data.Entities;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Shared
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : AEntityBase
    {
        protected readonly AppDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private bool _disposed;

        public RepositoryBase(AppDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public List<TEntity> GetAll() => _dbContext.Set<TEntity>().ToList();

        public TEntity? Find(Func<TEntity, bool> predicate)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        public List<TEntity> GetBy(Func<TEntity, bool> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate).ToList();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
            => await _dbContext.Set<TEntity>().FindAsync(id);

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

        public async Task SaveChangesAsync(string? currentUserName = null)
        {
            var userName = _currentUserService.CurrentUser?.Username ?? "System";

            var modifiedEntries = _dbContext.ChangeTracker
                .Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is AEntityBase entity)
                {
                    var now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = now;
                        entity.CreatedBy = userName;
                        entity.UpdatedAt = now;
                        entity.UpdatedBy = userName;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = now;
                        entity.UpdatedBy = userName;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
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
