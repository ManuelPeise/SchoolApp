

using Data.Entities;

namespace Logic.Shared.Interfaces
{
    public interface IRepositoryBase<T>: IDisposable where T : AEntityBase
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAsync(Func<T, bool> predicate);
        Task InsertAsync(T entity, Func<T, bool> predicate);
        Task Update(T entity);
        Task DeleteAsync(int id);
        Task SaveChanges();
    }
}
