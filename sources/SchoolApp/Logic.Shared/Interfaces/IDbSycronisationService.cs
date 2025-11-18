using Data.Entities;

namespace Logic.Shared.Interfaces
{
    public interface IDbSycronisationService: IDisposable
    {
        Task<DatabaseModel?> GetMySqlDbModel(List<int> userIds);
        Task CreateUserRelatedMySqlTableEntries(int userId);
    }
}
