using Logic.Shared.Interfaces;
using Shared.Models;
using Shared.Models.Import;

namespace Logic.Import
{
    public class JsonFileImporter : IJsonFileImporter
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICurrentUserService _currentUserService;
        private bool disposedValue;

        public JsonFileImporter(IDbContextFactory dbContextFactory, ICurrentUserService currentUserService)
        {
            _dbContextFactory = dbContextFactory;
            _currentUserService = currentUserService;
        }

        public async Task<ResponseBaseModel> ImportJson(FileImportModel model)
        {
            var instance = FileImportFactory.Execute(model, _dbContextFactory, _currentUserService);

            return await instance.Execute();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _currentUserService.Dispose();
                    _dbContextFactory.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
