using Data.ContextMysql;
using Data.Entities;
using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.User;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Shared
{
    public class ApplicationUnitOfWorkMySql: IApplicationUnitOfWorkMySql
    {
        private bool disposedValue;
        private readonly MySqlDbContext _mySqlDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepositoryBaseMySql<FamilyEntity> _familyRepository;
        private readonly IRepositoryBaseMySql<AppUserEntity> _userRepository;
        private readonly IRepositoryBaseMySql<LogEntryEntity> _logRepository;
        private readonly IRepositoryBaseMySql<LearnTopicEntity> _learnTopicRepository;
        private readonly IRepositoryBaseMySql<UserLearnTopicEntity> _userLearnTopicRepository;
        private readonly IRepositoryBaseMySql<VocabularyEntity> _vocabularyRepository;

        public IRepositoryBaseMySql<FamilyEntity> FamilyRepository => _familyRepository ?? new RepositoryBaseMysql<FamilyEntity>(_mySqlDbContext);
        public IRepositoryBaseMySql<AppUserEntity> UserRepository => _userRepository ?? new RepositoryBaseMysql<AppUserEntity>(_mySqlDbContext);
        public IRepositoryBaseMySql<LogEntryEntity> LogRepository => _logRepository ?? new RepositoryBaseMysql<LogEntryEntity>(_mySqlDbContext);
        public IRepositoryBaseMySql<LearnTopicEntity> LearnTopicRepository => _learnTopicRepository ?? new RepositoryBaseMysql<LearnTopicEntity>(_mySqlDbContext);
        public IRepositoryBaseMySql<UserLearnTopicEntity> UserLearnTopicRepository => _userLearnTopicRepository ?? new RepositoryBaseMysql<UserLearnTopicEntity>(_mySqlDbContext);
        public IRepositoryBaseMySql<VocabularyEntity> VocabularyRepository => _vocabularyRepository ?? new RepositoryBaseMysql<VocabularyEntity>(_mySqlDbContext);

        public ApplicationUnitOfWorkMySql(MySqlDbContext mySqlDbContext, ICurrentUserService currentUserService)
        {
            _mySqlDbContext = mySqlDbContext;
            _familyRepository = new RepositoryBaseMysql<FamilyEntity>(_mySqlDbContext);
            _userRepository = new RepositoryBaseMysql<AppUserEntity>(_mySqlDbContext);
            _logRepository = new RepositoryBaseMysql<LogEntryEntity>(_mySqlDbContext);
            _learnTopicRepository = new RepositoryBaseMysql<LearnTopicEntity>(_mySqlDbContext);
            _userLearnTopicRepository = new RepositoryBaseMysql<UserLearnTopicEntity>(_mySqlDbContext);
            _vocabularyRepository = new RepositoryBaseMysql<VocabularyEntity>(_mySqlDbContext);
            _currentUserService = currentUserService;
        }

        public async Task SaveChangesAsync()
        {
            var userName = _currentUserService.CurrentUser?.Username ?? "System";

            var modifiedEntries = _mySqlDbContext.ChangeTracker
                .Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is AEntityBase entity)
                {
                    var now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = now;
                        entity.CreatedBy = userName;
                        entity.UpdatedAt = now;
                        entity.UpdatedBy = userName;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = now;
                        entity.UpdatedBy = userName;
                    }
                }
            }

            await _mySqlDbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _mySqlDbContext.Dispose();
                    _logRepository.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
