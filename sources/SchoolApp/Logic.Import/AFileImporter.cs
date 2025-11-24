using Logic.Shared.Interfaces;
using Shared.Models;

namespace Logic.Import
{
    internal abstract class AFileImporter
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICurrentUserService _currentUserService;

        public IDbContextFactory DbContextFactory => _dbContextFactory;
        public ICurrentUserService CurrentUserService => _currentUserService;

        protected AFileImporter(IDbContextFactory dbContextFactory, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _dbContextFactory = dbContextFactory;
        }

        public abstract Task<ResponseBaseModel> Execute();
    }
}
