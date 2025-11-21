using Logic.Shared.Interfaces;
using Shared.Models;
using Shared.Models.Import;

namespace Logic.Import
{
    public class JsonFileImporter : IJsonFileImporter
    {
        private bool disposedValue;
        private readonly IApplicationUnitOfWorkMySql _applicationUnitOfWorkMySql;

        public JsonFileImporter(IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql)
        {
            _applicationUnitOfWorkMySql = applicationUnitOfWorkMySql;
        }

        public async Task<ResponseBaseModel> ImportJson(FileImportModel model)
        {
            var instance = FileImportFactory.Execute(model, _applicationUnitOfWorkMySql);

            return await instance.Execute();
        }

        

  
        
       

        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationUnitOfWorkMySql.Dispose();
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
