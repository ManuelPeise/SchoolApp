using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Import;

namespace Service.Api.Controllers.Import
{
    // [JwtAuth(UserRole = UserRoleEnum.Admin)]
    public class JsonImportController: ApiControllerBase
    {
        private readonly IJsonFileImporter _jsonFileImporter;

        public JsonImportController(IJsonFileImporter jsonFileImporter)
        {
            _jsonFileImporter = jsonFileImporter;
        }

        [HttpPost(Name ="ImportJsonFile")]
        public async Task ImportJsonFile(FileImportModel model)
        {
            await _jsonFileImporter.ImportJson(model);
        }
    }
}
