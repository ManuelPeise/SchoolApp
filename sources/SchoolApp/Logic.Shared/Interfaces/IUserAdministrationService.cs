using Data.Entities;
using Logic.Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IUserAdministrationService: IDisposable
    {
        Task<DatabaseModel?> RegisterUser(UserRegistrationRequestModel model);
    }
}
