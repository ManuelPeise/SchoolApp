using Logic.Shared.Interfaces;
using Shared.Models;

namespace Logic.Import
{
    internal abstract class AFileImporter
    {
        private readonly IApplicationUnitOfWorkMySql _applicationUnitOfWorkMySql;

        public IApplicationUnitOfWorkMySql UnitOfWork => _applicationUnitOfWorkMySql;

        protected AFileImporter(IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql)
        {
            _applicationUnitOfWorkMySql = applicationUnitOfWorkMySql;
        }

        public abstract Task<ResponseBaseModel> Execute();
    }
}
