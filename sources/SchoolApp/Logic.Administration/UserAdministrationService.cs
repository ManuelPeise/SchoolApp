using Data.Entities.Administration;
using Logic.Shared;
using Logic.Shared.Extensions;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Shared.Enums;
using System;

namespace Logic.Administration
{
    public class UserAdministrationService : IUserAdministrationService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IApplicationUnitOfWorkMySql _applicationUnitOfWorkMySql;

        public UserAdministrationService(IApplicationUnitOfWork applicationUnitOfWork, IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _applicationUnitOfWorkMySql = applicationUnitOfWorkMySql;
        }


        public async Task RegisterUser(ObservableUser user)
        {
            try
            {
                if (user == null) 
                {
                    await _applicationUnitOfWorkMySql.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = "Could not register user, user is null",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Error
                    }, null);

                    await _applicationUnitOfWorkMySql.SaveChangesAsync();

                    return;
                }

                var entity = user.ToEntity();

                entity.Salt = Guid.NewGuid().ToString();
                entity.Password = PasswordHelper.HashPassword(entity.Password, entity.Salt);

                await _applicationUnitOfWorkMySql.UserRepository
                    .AddAsync(entity, x => x.Username == user.Username);

                await _applicationUnitOfWorkMySql.SaveChangesAsync();

                var userEntity = _applicationUnitOfWorkMySql.UserRepository
                    .Find(x => x.Username == user.Username && x.DateOfBirth == entity.DateOfBirth);
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
            }
        }
    }
}
