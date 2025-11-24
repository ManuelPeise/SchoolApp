using Data.Context;
using Data.ContextMysql;
using Data.Shared;
using Logic.Shared.Interfaces;
using Shared.Enums;

namespace Logic.Shared
{
    public class DbContextFactory: IDbContextFactory
    {
        private readonly SqLiteDbContext _sqLiteDbContext;
        private readonly MySqlDbContext _mySqlDbContext;
        private bool disposedValue;

        public DbContextFactory(SqLiteDbContext sqLiteDbContext, MySqlDbContext mySqlDbContext)
        {
            _mySqlDbContext = mySqlDbContext;
            _sqLiteDbContext = sqLiteDbContext;
        }

        public ADatabaseContext CreateDbContext(DatabaseProviderTypeEnum providerType)
        {
            switch (providerType)
            {
                case DatabaseProviderTypeEnum.MySql:
                    return _mySqlDbContext;
                case DatabaseProviderTypeEnum.SqLite:
                    return _sqLiteDbContext;
                default:
                    throw new NotSupportedException($"The database provider type '{providerType}' is not supported.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                   
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
