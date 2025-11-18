using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.User;

namespace Logic.Shared.Interfaces
{
    public interface IApplicationUnitOfWorkMySql : IDisposable
    {
        IRepositoryBaseMySql<AppUserEntity> UserRepository { get; }
        IRepositoryBaseMySql<LogEntryEntity> LogRepository { get; }
        IRepositoryBaseMySql<LearnTopicEntity> LearnTopicRepository { get; }
        IRepositoryBaseMySql<UserLearnTopicEntity> UserLearnTopicRepository { get; }
        IRepositoryBaseMySql<VocabularyEntity> VocabularyRepository { get; }
        Task SaveChangesAsync();
    }
}
