using Logic.Shared.Storage;
using Shared.Models;

namespace Logic.Import
{
    internal abstract class AFileImporter
    {
        private readonly IRemoteDatabaseAccessor _databaseAccessor;

        public IRemoteDatabaseAccessor DatabaseAccessor => _databaseAccessor;

        protected AFileImporter(IRemoteDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public abstract Task<ResponseBaseModel> Execute();
    }
}
