using Data.Entities.Administration;
using Logic.Shared.Interfaces;

namespace Logic.Shared.Services
{
    public class LogService : ILogService
    {
        private bool disposedValue;
        private readonly IRepositoryBase<LogEntryEntity> _logRepository;
        
        public LogService(IRepositoryBase<LogEntryEntity> logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<List<LogEntryEntity>> GetLogMessages()
        {
            return await Task.FromResult(_logRepository.GetAll());
        }

        public async Task LogMessage(LogEntryEntity entity)
        {
            await _logRepository.AddAsync(entity, null);

            await _logRepository.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
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
