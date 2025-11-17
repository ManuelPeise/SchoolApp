using Data.Entities.Administration;

namespace Logic.Shared.Interfaces
{
    public interface ILogService: IDisposable
    {
        Task<List<LogEntryEntity>> GetLogMessages();
        Task LogMessage(LogEntryEntity entity);
    }
}
