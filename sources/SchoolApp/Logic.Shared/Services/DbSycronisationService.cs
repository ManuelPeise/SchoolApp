using Data.Entities;
using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Logic.Shared.Interfaces;
using Shared.Enums;

namespace Logic.Shared.Services
{
    public class DbSycronisationService : IDbSycronisationService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICurrentUserService _currentUserService;
        private bool disposedValue;

        public DbSycronisationService(IDbContextFactory dbContextFactory, ICurrentUserService currentUserService)
        {

            _dbContextFactory = dbContextFactory;
            _currentUserService = currentUserService;
        }

        public async Task<DatabaseModel?> GetMySqlDbModel(List<int> userIds)
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.MySql, _dbContextFactory, _currentUserService);

            var userEntities = await unitOfWork.UserRepository.GetBy(x => userIds.Contains(x.Id));

            if (!userEntities.Any())
            {
                await unitOfWork.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = "Could not get user from database",
                    ExceptionMessage = string.Empty,
                    Stacktrace = string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql);

                return null;
            }

            var topics = await unitOfWork.LearnTopicRepository.GetAll();
            var userTopics = await unitOfWork.UserLearnTopicRepository.GetBy(x => userIds.Contains(x.UserId));
            var vocabulary = await unitOfWork.VocabularyRepository.GetAll();

            return new DatabaseModel
            {
                AppUsers = userEntities,
                UserLearnTopics = userTopics,
                LearnTopics = topics,
                Vocabularys = vocabulary,
                Logs = new List<LogEntryEntity>()
            };
        }

        public async Task CreateUserRelatedMySqlTableEntries(int userId)
        {
            var hasChanges = false;

            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.MySql, _dbContextFactory, _currentUserService);

            var topics = await unitOfWork.LearnTopicRepository.GetAll();

            foreach (var topic in topics)
            {
                await unitOfWork.UserLearnTopicRepository.AddAsync(new UserLearnTopicEntity
                {
                    UserId = userId,
                    TopicId = topic.Id,
                    CanView = false,
                    CanEdit = false,
                    Deny = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System",

                }, null);

                hasChanges = true;
            }

            if (hasChanges)
            {
                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _currentUserService.Dispose();
                    _dbContextFactory.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
