using Data.Entities.User;
using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IProfileService: IDisposable
    {
        Task<ResponseBaseModel> ChangeProfileLocal(AppUserEntity entityToUpdate);
        Task<ResponseBaseModel> ChangeProfileRemote(AppUserEntity entityToUpdate);
    }
}
