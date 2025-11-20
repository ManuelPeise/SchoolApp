using Microsoft.AspNetCore.Http;

namespace Logic.Shared.Interfaces
{
    public interface IJsonFileImporter: IDisposable
    {
        Task ImportFamilyJsonTemplate(IFormFile file);
        Task ImportVocabularyFile(IFormFile file);
    }
}
