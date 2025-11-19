using Logic.Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IUserAdministrationService: IDisposable
    {
        Task<ResponseModelBase> RegisterUser(UserRegistrationRequestModel model);
    }
}
