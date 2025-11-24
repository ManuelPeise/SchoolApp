using Data.Entities.Administration;
using Logic.Shared.Interfaces;
using Shared.Enums;

namespace Logic.Shared.Services
{
    public class LogService : ILogService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICurrentUserService _currentUserService;
        private bool disposedValue;

        public LogService(IDbContextFactory dbContextFactory, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<LogEntryEntity>> GetLogMessagesFromSqLite()
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.SqLite, _dbContextFactory, _currentUserService);

            return await unitOfWork.LogRepository.GetAll();

        }

        public async Task<List<LogEntryEntity>> GetLogMessagesFromMySql()
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.MySql, _dbContextFactory, _currentUserService);

            return await unitOfWork.LogRepository.GetAll();
        }

        public async Task LogMessageSqLite(LogEntryEntity entity)
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.SqLite, _dbContextFactory, _currentUserService);

            await unitOfWork.LogRepository.AddAsync(entity, null);

            await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.SqLite);
        }


        public async Task LogMessageMySql(LogEntryEntity entity)
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.MySql, _dbContextFactory, _currentUserService);

            await unitOfWork.LogRepository.AddAsync(entity, null);

            await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql);
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
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
