using Data.Entities;
using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Logic.Shared.Interfaces;
using Shared.Enums;

namespace Logic.Shared.Services
{
    public class DbSycronisationService : IDbSycronisationService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IApplicationUnitOfWorkMySql _applicationUnitOfWorkMySql;
        private bool disposedValue;

        public DbSycronisationService(IApplicationUnitOfWork applicationUnitOfWork, IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _applicationUnitOfWorkMySql = applicationUnitOfWorkMySql;
        }

        public async Task<DatabaseModel?> GetMySqlDbModel(List<int> userIds)
        {
            var userEntities = await _applicationUnitOfWork.UserRepository.GetBy(x => userIds.Contains(x.Id));
            

            if (!userEntities.Any())
            {
                await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = "Could not get user from database",
                    ExceptionMessage = string.Empty,
                    Stacktrace = string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await _applicationUnitOfWorkMySql.SaveChangesAsync();

                return null;
            }

            var topics = _applicationUnitOfWorkMySql.LearnTopicRepository.GetAll();
            var userTopics = _applicationUnitOfWorkMySql.UserLearnTopicRepository.GetBy(x => userIds.Contains(x.UserId));
            var vocabulary = _applicationUnitOfWorkMySql.VocabularyRepository.GetAll();

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
            var topics = _applicationUnitOfWorkMySql.LearnTopicRepository.GetAll();

            foreach (var topic in topics)
            {
                await _applicationUnitOfWorkMySql.UserLearnTopicRepository.AddAsync(new UserLearnTopicEntity
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
                await _applicationUnitOfWorkMySql.SaveChangesAsync();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationUnitOfWork.Dispose();
                    _applicationUnitOfWorkMySql.Dispose();
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
