using Data.ContextMysql;
using Data.Entities;
using Logic.Shared.Interfaces;

namespace Logic.Shared
{
    public class RepositoryBaseMysql<TEntity> : IRepositoryBaseMySql<TEntity> where TEntity : AEntityBase
    {
        protected readonly MySqlDbContext _dbContext;
      
        private bool _disposed;

        public RepositoryBaseMysql(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
           
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
        
        public async Task<int?> GetEntityId(Func<TEntity, bool> predicate)
        {
            return await Task.FromResult(_dbContext.Set<TEntity>().Where(predicate).FirstOrDefault()?.Id);
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
