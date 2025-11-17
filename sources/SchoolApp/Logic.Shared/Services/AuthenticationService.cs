using Data.Entities.Administration;
using Data.Entities.User;
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
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepositoryBase<AppUserEntity> _userRepository;
        private readonly ILogService _logService;

        private bool disposedValue;

        public AuthenticationService(
            ICurrentUserService currentUserService,
            IRepositoryBase<AppUserEntity> userRepository,
            ILogService logService)
        {
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _logService = logService;
        }

        public async Task<ObservableCollection<ObservableUser>> GetUsers()
        {
            try
            {
                var users = _userRepository.GetAll();

                if (!users.Any())
                {
                    await _logService.LogMessage(new LogEntryEntity
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
                await _logService.LogMessage(new LogEntryEntity
                {
                    Message = "Could not load users from database!",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                return new ObservableCollection<ObservableUser>();
            }

        }

        public async Task<AuthenticationResult> Login(string userName, string password)
        {
            try
            {
                var user = _userRepository.Find(x => x.Username == userName);

                if (user == null)
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        ErrorMessage = "Could not find user for authentication."
                    };
                }

                if (user.Password != PasswordHelper.HashPassword(password, user.Salt))
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        ErrorMessage = "Could not authenticate user check username and password."
                    };
                }

                _currentUserService.SetCurrentUser(user);

                return new AuthenticationResult
                {
                    Success = true
                };
            }
            catch (Exception exception)
            {
                await _logService.LogMessage(new LogEntryEntity
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
            _currentUserService?.SetCurrentUser(null);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _userRepository.Dispose();
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
