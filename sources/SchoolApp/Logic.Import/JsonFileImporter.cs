using Logic.Shared.Interfaces;
using Logic.Shared.Storage;
using Shared.Models;
using Shared.Models.Import;

namespace Logic.Import
{
    public class JsonFileImporter : IJsonFileImporter
    {
        private readonly IRemoteDatabaseAccessor _databaseAccessor;

        private bool disposedValue;

        public JsonFileImporter(IRemoteDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;

        }

        public async Task<ResponseBaseModel> ImportJson(FileImportModel model)
        {
            var instance = FileImportFactory.Execute(model, _databaseAccessor);

            return await instance.Execute();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _databaseAccessor.Dispose();
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
