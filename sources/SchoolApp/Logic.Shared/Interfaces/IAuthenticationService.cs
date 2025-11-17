using Logic.Shared.Models;
using Shared.Models;
using System.Collections.ObjectModel;

namespace Logic.Shared.Interfaces
{
    public interface IAuthenticationService: IDisposable
    {
        Task<ObservableCollection<ObservableUser>> GetUsersFromSqLite();
        Task<ObservableCollection<ObservableUser>> GetUsersFromMySql();
        Task<AuthenticationResult> Login(string userName, string password);
        void LogOut();
    }
}
