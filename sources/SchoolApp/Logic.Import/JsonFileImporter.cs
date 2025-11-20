using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.User;
using Logic.Import.Models;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shared.Enums;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public async Task ImportFamilyJsonTemplate(IFormFile file)
        {
            try
            {
                var json = await GetJsonStringFromFile(file);

                var model = JsonConvert.DeserializeObject<FamilyImportModel>(json);

                if (model == null)
                {
                    await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = $"Error, file: {file.Name} is empty",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    }, null);

                    await _applicationUnitOfWorkMySql.SaveChangesAsync();

                    return;
                }


                if (!model.IsValidModel())
                {
                    await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = $"Could not import file: {file.Name} - model is invalid.",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    }, null);

                    await _applicationUnitOfWorkMySql.SaveChangesAsync();

                    return;
                }

                var familyId = await _applicationUnitOfWorkMySql.FamilyRepository.GetEntityId(x => x.FamilyName.ToLower() == model.FamilyName.ToLower());

                var importTimeStamp = DateTime.UtcNow;

                if (familyId == null)
                {
                    var familyMembers = await GetFamilyMemberEntities(model.FamilyMembers, model.FamilyName, importTimeStamp);

                    var adminUser = familyMembers.FirstOrDefault(x => x.UserRole == UserRoleEnum.Admin);

                    if (adminUser == null)
                    {
                        await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                        {
                            Message = $"Could not import family: {file.Name} - admin user is not defined.",
                            ExceptionMessage = string.Empty,
                            Stacktrace = string.Empty,
                            LogLevel = LogLevelEnum.Info
                        }, null);

                        await _applicationUnitOfWorkMySql.SaveChangesAsync();

                        return;
                    }

                    var familyEntity = new FamilyEntity
                    {
                        FamilyName = $"{adminUser.Username}.{model.FamilyName}",
                        FamilyDisplayName = model.FamilyName,
                        FamilyMembers = familyMembers,
                        CreatedAt = importTimeStamp,
                        CreatedBy = "System"
                    };

                    await _applicationUnitOfWorkMySql.FamilyRepository.AddAsync(familyEntity, null);
                   
                }

                await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = $"File: {file.Name} imported with success.",
                    ExceptionMessage = string.Empty,
                    Stacktrace = string.Empty,
                    LogLevel = LogLevelEnum.Info
                }, null);

                await _applicationUnitOfWorkMySql.SaveChangesAsync();

            }
            catch (Exception exception)
            {
                await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = $"Could not import file: {file.Name} - family import failed",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await _applicationUnitOfWorkMySql.SaveChangesAsync();
            }
        }

        public async Task ImportVocabularyFile(IFormFile file)
        {
            try
            {
                var json = await GetJsonStringFromFile(file);

                var model = JsonConvert.DeserializeObject<VocabularyTopicImportModel>(json);

                if (model == null)
                {
                    await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = $"Error, file: {file.Name} is empty",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    }, null);

                    await _applicationUnitOfWorkMySql.SaveChangesAsync();

                    return;
                }


                if (!model.IsValidTopic())
                {

                    await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = $"Could not import file: {file.Name} - topic is empty.",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    }, null);

                    await _applicationUnitOfWorkMySql.SaveChangesAsync();

                    return;
                }

                var topicId = await _applicationUnitOfWorkMySql.LearnTopicRepository.GetEntityId(x => x.TopicName.ToLower() == model.TopicName.ToLower());

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

                    await _applicationUnitOfWorkMySql.LearnTopicRepository.AddAsync(topicentity, null);

                    await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = $"File: {file.Name} imported with success.",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    }, null);

                    await _applicationUnitOfWorkMySql.SaveChangesAsync();

                    return;
                }

                var vocabulatyTopicId = topicId ?? await _applicationUnitOfWorkMySql.LearnTopicRepository.GetEntityId(x => x.TopicName.ToLower() == model.TopicName.ToLower());

                if (vocabulatyTopicId == null)
                {
                    return;
                }

                var existingVocabularyEntities = await _applicationUnitOfWorkMySql.VocabularyRepository.GetBy(x => x.TopicId == vocabulatyTopicId);

                var vocabularyEntities = GetVocabulariesToImport(model.Vocabularies, importTimeStamp, existingVocabularyEntities);

                vocabularyEntities.ForEach(e => e.TopicId = (int)vocabulatyTopicId);

                await _applicationUnitOfWorkMySql.VocabularyRepository.AddRangeAsync(vocabularyEntities);

                await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = $"File: {file.Name} imported with success.",
                    ExceptionMessage = string.Empty,
                    Stacktrace = string.Empty,
                    LogLevel = LogLevelEnum.Info
                }, null);

                await _applicationUnitOfWorkMySql.SaveChangesAsync();

            }
            catch (Exception exception)
            {
                await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = $"Could not import file: {file.Name}",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await _applicationUnitOfWorkMySql.SaveChangesAsync();
            }
        }

        private async Task<List<AppUserEntity>> GetFamilyMemberEntities(List<FamilyMemberImportModel> familyMembers, string familyName, DateTime timeStamp)
        {
            var members = new List<AppUserEntity>();

            foreach (var familyMember in familyMembers)
            {
                var userId = await _applicationUnitOfWorkMySql.UserRepository.GetEntityId(x =>
                    x.Username.ToLower() == familyMember.Name.ToLower() && x.LastName.ToLower() == familyName.ToLower());

                if (userId == null)
                {
                    var salt = Guid.NewGuid().ToString();

                    members.Add(new AppUserEntity
                    {
                        LastName = familyName,
                        Username = familyMember.Name,
                        DateOfBirth = familyMember.DateOfBirth,
                        UserRole = (UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), familyMember.UserRole),
                        Salt = salt,
                        Password = PasswordHelper.HashPassword(familyMember.Password, salt),
                        IsActive = familyMember.IsActive,
                        RefreshToken = string.Empty,
                        CreatedAt = timeStamp,
                        CreatedBy = "System"
                    });
                }

            }

            return members;
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

        private async Task<string> GetJsonStringFromFile(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
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
