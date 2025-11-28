using Data.Entities.User;
using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IProfileService: IDisposable
    {
        Task<ResponseBaseModel> ChangeProfile(AppUserEntity entityToUpdate);
    }
}
