using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.Settings;
using Data.Entities.User;
using Logic.Shared.Interfaces;

namespace Logic.Shared.Storage
{
    public interface IRemoteDatabaseAccessor: IDisposable
    {
        IRepositoryBase<FamilyEntity> FamilyRepository { get; }
        IRepositoryBase<AppUserEntity> UserRepository { get; }
        IRepositoryBase<AppUserCredentialsEntity> UserCredentialsRepository { get; }
        IRepositoryBase<LogEntryEntity> LogRepository { get; }
        IRepositoryBase<LearnTopicEntity> LearnTopicRepository { get; }
        IRepositoryBase<UserLearnTopicEntity> UserLearnTopicRepository { get; }
        IRepositoryBase<VocabularyEntity> VocabularyRepository { get; }
        IRepositoryBase<SettingsEntity> SettingsRepository { get; }
        Task<List<LogEntryEntity>> GetLogMessages();
        Task LogMessage(LogEntryEntity entity);
        Task SaveChangesAsync();
    }
}
