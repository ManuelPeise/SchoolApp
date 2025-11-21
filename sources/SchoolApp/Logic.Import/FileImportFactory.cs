using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Models.Import;

namespace Logic.Import
{
    internal static class FileImportFactory
    {
        internal static AFileImporter Execute(FileImportModel model, IApplicationUnitOfWorkMySql unitOfWork)
        {
            switch (model.FileType)
            {
                case FileTypeEnum.FamilyJsonFile:
                    return new FamilyFileImporter(model, unitOfWork);
                case FileTypeEnum.VocabularyJsonFile:
                    return new VocabularyFileImporter(model, unitOfWork);
                default: throw new ArgumentOutOfRangeException(nameof(model.FileType));
            }
        }
    }
}
