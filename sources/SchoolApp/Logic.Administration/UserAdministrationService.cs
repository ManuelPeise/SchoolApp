using Data.Entities;
using Data.Entities.Administration;
using Logic.Shared;
using Logic.Shared.Extensions;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Shared.Enums;

namespace Logic.Administration
{
    public class UserAdministrationService : IUserAdministrationService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IApplicationUnitOfWorkMySql _applicationUnitOfWorkMySql;
        private readonly IDbSycronisationService _dbSycronisationService;
        private bool disposedValue;

        public UserAdministrationService(
            IApplicationUnitOfWork applicationUnitOfWork, 
            IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql, 
            IDbSycronisationService dbSycronisationService)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _applicationUnitOfWorkMySql = applicationUnitOfWorkMySql;
            _dbSycronisationService = dbSycronisationService;
        }


        public async Task<DatabaseModel?> RegisterUser(UserRegistrationRequestModel model)
        {
            try
            {
                if (model.User == null) 
                {
                    await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = "Could not register user, user is null",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Error
                    }, null);

                    await _applicationUnitOfWorkMySql.SaveChangesAsync();

                    return null;
                }

                var entity = model.User.ToEntity();

                entity.Salt = Guid.NewGuid().ToString();
                entity.Password = PasswordHelper.HashPassword(entity.Password, entity.Salt);

                await _applicationUnitOfWorkMySql.UserRepository
                    .AddAsync(entity, x => x.Username == model.User.Username);

                await _applicationUnitOfWorkMySql.SaveChangesAsync();

                var userId = await _applicationUnitOfWorkMySql.UserRepository
                    .GetEntityId(x => x.Username == model.User.Username && x.DateOfBirth == entity.DateOfBirth);

                if(userId == null)
                {
                    await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = "Could not get user from database",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Error
                    }, null);

                    await _applicationUnitOfWorkMySql.SaveChangesAsync();

                    return null;
                }

                await _dbSycronisationService.CreateUserRelatedMySqlTableEntries((int)userId);

                return await _dbSycronisationService.GetMySqlDbModel(model.UserIds);
            }
            catch (Exception exception)
            {
                await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = "User registration failed",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await _applicationUnitOfWorkMySql.SaveChangesAsync();

                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationUnitOfWork.Dispose();
                    _applicationUnitOfWorkMySql.Dispose();
                    _dbSycronisationService.Dispose();
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
