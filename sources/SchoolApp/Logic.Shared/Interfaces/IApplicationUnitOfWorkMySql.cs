using Data.Entities.Administration;
using Data.Entities.User;

namespace Logic.Shared.Interfaces
{
    public interface IApplicationUnitOfWorkMySql : IDisposable
    {
        public IRepositoryBaseMySql<AppUserEntity> UserRepository { get; }
        public IRepositoryBaseMySql<LogEntryEntity> LogRepository { get; }
        Task SaveChangesAsync();
    }
}
