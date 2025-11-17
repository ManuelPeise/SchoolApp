using Data.Entities;

namespace Logic.Shared.Interfaces
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : AEntityBase
    {
        List<TEntity> GetAll();
        TEntity? Find(Func<TEntity, bool> predicate);
        List<TEntity> GetBy(Func<TEntity, bool> predicate);
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity, Func<TEntity, bool>? predicate);
        void Update(TEntity entity);
        Task RemoveAsync(int id);
        Task SaveChangesAsync(string? currentUserName = null);
    }
}
