using Data.Entities.Administration;

namespace Logic.Shared.Interfaces
{
    public interface ILogService: IDisposable
    {
        Task<List<LogEntryEntity>> GetLogMessagesFromSqLite();
        Task<List<LogEntryEntity>> GetLogMessagesFromMySql();
        Task LogMessageSqLite(LogEntryEntity entity);
        Task LogMessageMySql(LogEntryEntity entity);
    }
}
