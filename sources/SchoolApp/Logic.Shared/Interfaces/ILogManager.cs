using Data.Entities;

namespace Logic.Shared.Interfaces
{
    public interface ILogManager: IDisposable
    {
        Task<List<LogEntity>> GetLogMessages();
        Task AddLogMessage(LogEntity logEntity);
        Task DeleteLogMessage(int id);
        Task DeleteLogMessages(List<int> ids);
        Task SaveChanges();
    }
}
