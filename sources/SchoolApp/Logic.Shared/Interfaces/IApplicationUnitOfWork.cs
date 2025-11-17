using Data.Entities.Administration;
using Data.Entities.User;

namespace Logic.Shared.Interfaces
{
    public interface IApplicationUnitOfWork: IDisposable
    {
        public IRepositoryBase<AppUserEntity> UserRepository { get; }
        public IRepositoryBase<LogEntryEntity> LogRepository { get; }
        Task SaveChangesAsync();
    }
}
