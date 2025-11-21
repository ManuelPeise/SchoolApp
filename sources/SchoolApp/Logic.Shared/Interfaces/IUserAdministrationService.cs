using Logic.Shared.Models;
using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IUserAdministrationService: IDisposable
    {
        Task<ResponseBaseModel> RegisterUser(UserRegistrationRequestModel model);
    }
}
