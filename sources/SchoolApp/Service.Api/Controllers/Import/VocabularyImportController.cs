using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Service.Api.Controllers.Import
{
    // [JwtAuth(UserRole = UserRoleEnum.Admin)]
    public class VocabularyImportController: ApiControllerBase
    {
        private readonly IJsonFileImporter _jsonFileImporter;

        public VocabularyImportController(IJsonFileImporter jsonFileImporter)
        {
            _jsonFileImporter = jsonFileImporter;
        }

        [HttpPost("ImportVocabularyTopic")]
        public async Task ImportVocabularyTopic(IFormFile file)
        {
            await _jsonFileImporter.ImportVocabularyFile(file);
        }
    }
}
