using Data.Entities;
using System;
using System.Linq.Expressions;

namespace Logic.Shared.Interfaces
{
    public interface IRepositoryBase<TEntity>: IDisposable where TEntity : AEntityBase
    {
        Task<List<TEntity>> GetAll(bool asNoTracking = false);
        Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false, params Expression<Func<TEntity, object>>[]? includeExpressions);
        Task<List<TEntity>> GetBy(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false, params Expression<Func<TEntity, object>>[]? includeExpressions);
        Task<TEntity?> GetByIdAsync(int id, bool asNoTracking = false, params Expression<Func<TEntity, object>>[]? includeExpressions);
        Task<int?> GetEntityId(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false, params Expression<Func<TEntity, object>>[]? includeExpressions);
        Task AddAsync(TEntity entity, Func<TEntity, bool>? predicate, bool asNoTracking = false);
        Task AddRangeAsync(List<TEntity> entities, bool asNoTracking = false);
        void Update(TEntity entity, bool asNoTracking = false);
        Task RemoveAsync(int id, bool asNoTracking = false);
    }
}
