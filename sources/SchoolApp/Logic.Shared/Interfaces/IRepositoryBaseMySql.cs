using Data.Entities;
using System.Linq.Expressions;

namespace Logic.Shared.Interfaces
{
    public interface IRepositoryBaseMySql<TEntity> : IDisposable where TEntity : AEntityBase
    {
        Task<List<TEntity>> GetAll();
        Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetBy(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetByIdAsync(int id);
        Task<int?> GetEntityId(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity, Func<TEntity, bool>? predicate);
        Task AddRangeAsync(List<TEntity> entities);
        void Update(TEntity entity);
        Task RemoveAsync(int id);
    }
}
