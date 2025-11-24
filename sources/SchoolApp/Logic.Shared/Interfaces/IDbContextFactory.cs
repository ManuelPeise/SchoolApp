using Data.Shared;
using Shared.Enums;

namespace Logic.Shared.Interfaces
{
    public interface IDbContextFactory: IDisposable
    {
        ADatabaseContext CreateDbContext(DatabaseProviderTypeEnum providerType);
    }
}
