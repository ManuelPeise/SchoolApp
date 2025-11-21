using Shared.Models;
using Shared.Models.Import;

namespace Logic.Shared.Interfaces
{
    public interface IJsonFileImporter: IDisposable
    {
        Task<ResponseBaseModel> ImportJson(FileImportModel model);
    }
}
