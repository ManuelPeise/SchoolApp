using Data.Entities.Administration;
using Logic.Shared.Interfaces;

namespace Logic.Shared.Services
{
    public class LogService : ILogService
    {
        private bool disposedValue;
        private readonly IApplicationUnitOfWorkMySql _applicationUnitOfWorkMySql;
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public LogService(IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql, IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _applicationUnitOfWorkMySql = applicationUnitOfWorkMySql;
        }

        public async Task<List<LogEntryEntity>> GetLogMessagesFromSqLite()
        {
            return await _applicationUnitOfWork.LogRepository.GetAll();
        }

        public async Task<List<LogEntryEntity>> GetLogMessagesFromMySql()
        {
            return await Task.FromResult(_applicationUnitOfWorkMySql.LogRepository.GetAll());
        }

        public async Task LogMessageSqLite(LogEntryEntity entity)
        {
            await _applicationUnitOfWork.LogRepository.AddAsync(entity, null);

            await _applicationUnitOfWork.SaveChangesAsync();
        }


        public async Task LogMessageMySql(LogEntryEntity entity)
        {
            await _applicationUnitOfWorkMySql.LogRepository.AddAsync(entity, null);

            await _applicationUnitOfWorkMySql.SaveChangesAsync();
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
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
