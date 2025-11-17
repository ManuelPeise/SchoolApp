using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Shared.Interfaces
{
    public interface IRepositoryBaseMySql<TEntity> : IDisposable where TEntity : AEntityBase
    {
        List<TEntity> GetAll();
        TEntity? Find(Func<TEntity, bool> predicate);
        List<TEntity> GetBy(Func<TEntity, bool> predicate);
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity, Func<TEntity, bool>? predicate);
        void Update(TEntity entity);
        Task RemoveAsync(int id);
    }
}
