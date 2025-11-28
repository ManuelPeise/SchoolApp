using Data.Context;
using Data.ContextMysql;
using Data.Entities;
using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.User;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Logic.Shared.Storage
{
    public class RemoteDatabaseAccessor: ALogicBase, IRemoteDatabaseAccessor
    {
        private readonly MySqlDbContext _dbContext;
        private readonly IRepositoryBase<FamilyEntity> _familyRepository;
        private readonly IRepositoryBase<AppUserEntity> _userRepository;
        private readonly IRepositoryBase<AppUserCredentialsEntity> _userCredentialsRepository;
        private readonly IRepositoryBase<LogEntryEntity> _logRepository;
        private readonly IRepositoryBase<LearnTopicEntity> _learnTopicRepository;
        private readonly IRepositoryBase<UserLearnTopicEntity> _userLearnTopicRepository;
        private readonly IRepositoryBase<VocabularyEntity> _vocabularyRepository;
        private bool disposedValue;

        public IRepositoryBase<FamilyEntity> FamilyRepository => _familyRepository ?? new RepositoryBase<FamilyEntity>(_dbContext);
        public IRepositoryBase<AppUserEntity> UserRepository => _userRepository ?? new RepositoryBase<AppUserEntity>(_dbContext);
        public IRepositoryBase<AppUserCredentialsEntity> UserCredentialsRepository => _userCredentialsRepository ?? new RepositoryBase<AppUserCredentialsEntity>(_dbContext);
        public IRepositoryBase<LogEntryEntity> LogRepository => _logRepository ?? new RepositoryBase<LogEntryEntity>(_dbContext);
        public IRepositoryBase<LearnTopicEntity> LearnTopicRepository => _learnTopicRepository ?? new RepositoryBase<LearnTopicEntity>(_dbContext);
        public IRepositoryBase<UserLearnTopicEntity> UserLearnTopicRepository => _userLearnTopicRepository ?? new RepositoryBase<UserLearnTopicEntity>(_dbContext);
        public IRepositoryBase<VocabularyEntity> VocabularyRepository => _vocabularyRepository ?? new RepositoryBase<VocabularyEntity>(_dbContext);

        public RemoteDatabaseAccessor(MySqlDbContext dbContext, IHttpContextAccessor httpContextAccessor):base(httpContextAccessor)
        {
            _dbContext = dbContext;
            
            _familyRepository = new RepositoryBase<FamilyEntity>(_dbContext);
            _userRepository = new RepositoryBase<AppUserEntity>(_dbContext);
            _logRepository = new RepositoryBase<LogEntryEntity>(_dbContext);
            _learnTopicRepository = new RepositoryBase<LearnTopicEntity>(_dbContext);
            _userLearnTopicRepository = new RepositoryBase<UserLearnTopicEntity>(_dbContext);
            _vocabularyRepository = new RepositoryBase<VocabularyEntity>(_dbContext);
            _userCredentialsRepository = new RepositoryBase<AppUserCredentialsEntity>(_dbContext); 
        }

        public async Task<List<LogEntryEntity>> GetLogMessages()
        {
            return await _logRepository.GetAll();
        }

        public async Task LogMessage(LogEntryEntity entity)
        {
            await _logRepository.AddAsync(entity, null, false);
        }

        public async Task SaveChangesAsync()
        {
            var userName = CurrentUser?.UserName ?? "SystemAdmin";

            var modifiedEntries = _dbContext.ChangeTracker
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

            await _dbContext.SaveChangesAsync();
        }

        #region dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                    _familyRepository.Dispose();
                    _learnTopicRepository.Dispose();
                    _logRepository.Dispose();
                    _userCredentialsRepository.Dispose();
                    _userLearnTopicRepository.Dispose();
                    _userRepository.Dispose();
                    _vocabularyRepository.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
