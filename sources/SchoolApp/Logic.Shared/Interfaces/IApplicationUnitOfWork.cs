using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.User;

namespace Logic.Shared.Interfaces
{
    public interface IApplicationUnitOfWork: IDisposable
    {
        IRepositoryBase<FamilyEntity> FamilyRepository { get; }
        IRepositoryBase<AppUserEntity> UserRepository { get; }
        IRepositoryBase<LogEntryEntity> LogRepository { get; }
        IRepositoryBase<LearnTopicEntity> LearnTopicRepository { get; }
        IRepositoryBase<UserLearnTopicEntity> UserLearnTopicRepository { get; }
        IRepositoryBase<VocabularyEntity> VocabularyRepository { get; }
        Task SaveChangesAsync();
    }
}
