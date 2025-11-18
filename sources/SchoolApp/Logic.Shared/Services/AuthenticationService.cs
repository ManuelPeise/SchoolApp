using Data.Entities.Administration;
using Logic.Shared.Extensions;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Shared.Enums;
using Shared.Models;
using System.Collections.ObjectModel;

namespace Logic.Shared.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IApplicationUnitOfWorkMySql _applicationUnitOfWorkMySql;
        private readonly ILogService _logService;

        private bool disposedValue;

        public AuthenticationService(
            IApplicationUnitOfWork applicationUnitOfWork,
            IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql,
            ILogService logService)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _applicationUnitOfWorkMySql = applicationUnitOfWorkMySql;
            _logService = logService;
        }

        public async Task<ObservableCollection<ObservableUser>> GetUsersFromSqLite()
        {
            try
            {
                var users = await _applicationUnitOfWork.UserRepository.GetAll();

                if (!users.Any())
                {
                    await _logService.LogMessageSqLite(new LogEntryEntity
                    {
                        Message = "Could not find any users in database!",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    });

                    return new ObservableCollection<ObservableUser>();
                }

                var userList = new List<ObservableUser> {
                    new ObservableUser
                    {
                        Username = "Benutzer wählen",
                        UserRole = UserRoleEnum.None
                    }
                };

                var observableUsers = users.Select(x => x.ToObservable()).ToList();

                userList.AddRange(observableUsers);

                return new ObservableCollection<ObservableUser>(userList);
            }
            catch (Exception exception)
            {
                await _logService.LogMessageSqLite(new LogEntryEntity
                {
                    Message = "Could not load users from database!",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                return new ObservableCollection<ObservableUser>();
            }

        }

        public async Task<ObservableCollection<ObservableUser>> GetUsersFromMySql()
        {
            try
            {
                var users = _applicationUnitOfWorkMySql.UserRepository.GetAll();

                if (!users.Any())
                {
                    await _logService.LogMessageMySql(new LogEntryEntity
                    {
                        Message = "Could not find any users in database!",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    });

                    return new ObservableCollection<ObservableUser>();
                }

                var userList = new List<ObservableUser> {
                    new ObservableUser
                    {
                        Username = "Neuer Benutzer",
                        UserRole = UserRoleEnum.None
                    }
                };

                var observableUsers = users.Select(x => x.ToObservable()).ToList();

                userList.AddRange(observableUsers);

                return new ObservableCollection<ObservableUser>(userList);
            }
            catch (Exception exception)
            {
                await _logService.LogMessageMySql(new LogEntryEntity
                {
                    Message = "Could not load users from database!",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                return new ObservableCollection<ObservableUser>();
            }

        }

        public async Task<ObservableUser?> GetUserFromSqLite(string username)
        {
            var user = await _applicationUnitOfWork.UserRepository.Find(x => x.Username.ToLower() == username.ToLower());

            if(user == null) { return null; }

            return user.ToObservable();
        }

        public async Task<AuthenticationResult> LoginAsync(LoginRequestModel model)
        {
            try
            {
                var user = _applicationUnitOfWorkMySql.UserRepository.Find(x => x.Username == model.UserName);

                if (user == null)
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        ErrorMessage = "Could not find user for authentication."
                    };
                }

                if (user.Password != PasswordHelper.HashPassword(model.Password, user.Salt))
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        ErrorMessage = "Could not authenticate user check username and password."
                    };
                }

                return new AuthenticationResult
                {
                    Success = true,
                    User = user
                };
            }
            catch (Exception exception)
            {
                await _logService.LogMessageSqLite(new LogEntryEntity
                {
                    Message = "Authentication failed!",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Info
                });

                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = "Login failed!"
                };
            }
        }

        public async Task<AuthenticationResult> LoginLocalAsync(LoginRequestModel model)
        {
            try
            {
                var user = await _applicationUnitOfWork.UserRepository.Find(x => x.Username == model.UserName);

                if (user == null)
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        ErrorMessage = "Could not find user for authentication."
                        
                    };
                }

                if (user.Password != PasswordHelper.HashPassword(model.Password, user.Salt))
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        ErrorMessage = "Could not authenticate user check username and password."
                    };
                }

                

                return new AuthenticationResult
                {
                    Success = true,
                    User = user
                };
            }
            catch (Exception exception)
            {
                await _logService.LogMessageSqLite(new LogEntryEntity
                {
                    Message = "Authentication failed!",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Info
                });

                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = "Login failed!"
                };
            }
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _applicationUnitOfWork.Dispose();
                    _applicationUnitOfWorkMySql.Dispose();
                    _logService.Dispose();
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
