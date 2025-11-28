using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Sync;

namespace Service.Api.Controllers.Sync
{
    [JwtAuth(UserRoleString ="User, Admin, SystemAdmin")]
    public class DataSyncController: ApiControllerBase
    {
        private readonly IRemoteDataSyncService _remoteDataSyncService;

        public DataSyncController(IRemoteDataSyncService remoteDataSyncService)
        {
            _remoteDataSyncService = remoteDataSyncService;
        }

        [HttpPost(Name = "SyncUserData")]
        public async Task<AppUserSyncModel?> SyncUserData([FromBody] AppUserSyncModel syncModel)
        {
           return await _remoteDataSyncService.UpdateAppUserEntity(syncModel);
        }
    }
}
