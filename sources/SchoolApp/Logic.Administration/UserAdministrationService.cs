using Logic.Shared.Interfaces;

namespace Logic.Administration
{
    public class UserAdministrationService: IUserAdministrationService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IApplicationUnitOfWorkMySql _applicationUnitOfWorkMySql;

        public UserAdministrationService(IApplicationUnitOfWork applicationUnitOfWork, IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _applicationUnitOfWorkMySql = applicationUnitOfWorkMySql;
        }
    }
}
