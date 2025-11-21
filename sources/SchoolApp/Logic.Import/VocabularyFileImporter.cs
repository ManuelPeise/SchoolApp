using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Logic.Import.Models;
using Logic.Shared.Interfaces;
using Newtonsoft.Json;
using Shared.Enums;
using Shared.Models;
using Shared.Models.Import;

namespace Logic.Import
{
    internal class VocabularyFileImporter : AFileImporter
    {
        private readonly FileImportModel _fileImportModel;
        public VocabularyFileImporter(FileImportModel fileImportModel, IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql) : base(applicationUnitOfWorkMySql)
        {
            _fileImportModel = fileImportModel;
        }

        public override async Task<ResponseBaseModel> Execute()
        {
            try
            {
                if (string.IsNullOrEmpty(_fileImportModel.FileContent))
                {
                    await UnitOfWork.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = $"Error: file: {_fileImportModel.FileName} is empty",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    }, null);

                    await UnitOfWork.SaveChangesAsync();

                    return new ResponseBaseModel { Success = false, Message = $"Error: file: {_fileImportModel.FileName} is empty" };
                }

                var model = JsonConvert.DeserializeObject<VocabularyTopicImportModel>(_fileImportModel.FileContent);

                if (model == null)
                {
                    await UnitOfWork.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = $"Error, file: {_fileImportModel.FileName} is empty",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    }, null);

                    await UnitOfWork.SaveChangesAsync();

                    return new ResponseBaseModel { Success = false, Message = $"Error: file: {_fileImportModel.FileName} is empty" };
                }


                if (!model.IsValidTopic())
                {
                    await UnitOfWork.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = $"Error file: {_fileImportModel.FileName} could not be parsed.",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    }, null);

                    await UnitOfWork.SaveChangesAsync();

                    return new ResponseBaseModel { Success = false, Message = $"Error file: {_fileImportModel.FileName} could not be parsed." };
                }

                var topicId = await UnitOfWork.LearnTopicRepository.GetEntityId(x => x.TopicName.ToLower() == model.TopicName.ToLower());

                var importTimeStamp = DateTime.UtcNow;

                if (topicId == null)
                {
                    var vocabularies = GetVocabulariesToImport(model.Vocabularies, importTimeStamp);

                    var topicentity = new LearnTopicEntity
                    {
                        TopicName = model.TopicName,
                        TopicDescription = model.TopicDescription,
                        Vocabulary = vocabularies,
                        CreatedAt = importTimeStamp,
                        CreatedBy = "System"
                    };

                    await UnitOfWork.LearnTopicRepository.AddAsync(topicentity, null);

                    await UnitOfWork.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = $"File: {_fileImportModel.FileName} imported with success.",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    }, null);

                    await UnitOfWork.SaveChangesAsync();

                }

                var vocabulatyTopicId = topicId ?? await UnitOfWork.LearnTopicRepository.GetEntityId(x => x.TopicName.ToLower() == model.TopicName.ToLower());

                if (vocabulatyTopicId == null)
                {
                    return new ResponseBaseModel { Success = false, Message = $"Could not import file: {_fileImportModel.FileName} - topic defined topic not found." };
                }

                var existingVocabularyEntities = await UnitOfWork.VocabularyRepository.GetBy(x => x.TopicId == vocabulatyTopicId);

                var vocabularyEntities = GetVocabulariesToImport(model.Vocabularies, importTimeStamp, existingVocabularyEntities);

                vocabularyEntities.ForEach(e => e.TopicId = (int)vocabulatyTopicId);

                await UnitOfWork.VocabularyRepository.AddRangeAsync(vocabularyEntities);

                await UnitOfWork.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = $"File: {_fileImportModel.FileName} imported with success.",
                    ExceptionMessage = string.Empty,
                    Stacktrace = string.Empty,
                    LogLevel = LogLevelEnum.Info
                }, null);

                await UnitOfWork.SaveChangesAsync();

                return new ResponseBaseModel { Success = true, Message = $"Import file: {_fileImportModel.FileName} with success." };
            }
            catch (Exception exception)
            {
                await UnitOfWork.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = $"Could not import file: {_fileImportModel.FileName}",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await UnitOfWork.SaveChangesAsync();

                return new ResponseBaseModel { Success = false, Message = $"Error: file: {_fileImportModel.FileName} is empty" };
            }
        }

        private List<VocabularyEntity> GetVocabulariesToImport(List<VocabularyModel> vocabularies, DateTime timeStamp, List<VocabularyEntity>? existingEntities = null)
        {
            var entities = new List<VocabularyEntity>();

            foreach (var model in vocabularies)
            {
                if (!model.IsValid())
                {
                    continue;
                }

                var entity = existingEntities?.FirstOrDefault(x => x.German.ToLower() == model.German.ToLower());

                if (entity != null)
                {
                    entity.German = model.German;
                    entity.English = model.English;
                    entity.Danish = model.Danish;
                }
                else
                {
                    entity = new VocabularyEntity
                    {
                        German = model.German,
                        English = model.English,
                        Danish = model.Danish,
                        CreatedAt = timeStamp,
                        CreatedBy = "System"
                    };

                    entities.Add(entity);
                }
            }

            return entities;
        }
    }
}
