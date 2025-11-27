using Data.Entities;
using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.User;
using Data.Shared;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace Logic.Shared
{
    public class ApplicationUnitOfWork
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ADatabaseContext _dbContext;
        private readonly DatabaseProviderTypeEnum _providerType;

        private readonly IRepositoryBase<FamilyEntity> _familyRepository;
        private readonly IRepositoryBase<AppUserEntity> _userRepository;
        private readonly IRepositoryBase<AppUserCredentialsEntity> _userCredentialsRepository;
        private readonly IRepositoryBase<LogEntryEntity> _logRepository;
        private readonly IRepositoryBase<LearnTopicEntity> _learnTopicRepository;
        private readonly IRepositoryBase<UserLearnTopicEntity> _userLearnTopicRepository;
        private readonly IRepositoryBase<VocabularyEntity> _vocabularyRepository;

        public IRepositoryBase<FamilyEntity> FamilyRepository => _familyRepository ?? new RepositoryBase<FamilyEntity>(_dbContext ?? _dbContextFactory.CreateDbContext(_providerType));
        public IRepositoryBase<AppUserEntity> UserRepository => _userRepository ?? new RepositoryBase<AppUserEntity>(_dbContext ?? _dbContextFactory.CreateDbContext(_providerType));
        public IRepositoryBase<AppUserCredentialsEntity> UserCredentialsRepository => _userCredentialsRepository ?? new RepositoryBase<AppUserCredentialsEntity>(_dbContext ?? _dbContextFactory.CreateDbContext(_providerType));
        public IRepositoryBase<LogEntryEntity> LogRepository => _logRepository ?? new RepositoryBase<LogEntryEntity>(_dbContext ?? _dbContextFactory.CreateDbContext(_providerType));
        public IRepositoryBase<LearnTopicEntity> LearnTopicRepository => _learnTopicRepository ?? new RepositoryBase<LearnTopicEntity>(_dbContext ?? _dbContextFactory.CreateDbContext(_providerType));
        public IRepositoryBase<UserLearnTopicEntity> UserLearnTopicRepository => _userLearnTopicRepository ?? new RepositoryBase<UserLearnTopicEntity>(_dbContext ?? _dbContextFactory.CreateDbContext(_providerType));
        public IRepositoryBase<VocabularyEntity> VocabularyRepository => _vocabularyRepository ?? new RepositoryBase<VocabularyEntity>(_dbContext ?? _dbContextFactory.CreateDbContext(_providerType));

        public ApplicationUnitOfWork(DatabaseProviderTypeEnum providerType, IDbContextFactory dbContextFactory, ICurrentUserService currentUserService)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = _dbContextFactory.CreateDbContext(providerType);

            _providerType = providerType;

            _familyRepository = new RepositoryBase<FamilyEntity>(_dbContext);
            _userRepository = new RepositoryBase<AppUserEntity>(_dbContext);
            _logRepository = new RepositoryBase<LogEntryEntity>(_dbContext);
            _learnTopicRepository = new RepositoryBase<LearnTopicEntity>(_dbContext);
            _userLearnTopicRepository = new RepositoryBase<UserLearnTopicEntity>(_dbContext);
            _vocabularyRepository =  new RepositoryBase<VocabularyEntity>(_dbContext);
        }

        public async Task SaveChangesAsync(DatabaseProviderTypeEnum providerType, string? currentUser = null)
        {
            var userName = currentUser ?? "System";

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
    }
}
