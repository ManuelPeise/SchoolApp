using Data.Entities.User;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Service.Api.Controllers.Account
{
    public class ProfileController: ApiControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost(Name = "UpdateProfile")]
        public async Task<ResponseBaseModel> UpdateProfile([FromBody] AppUserEntity entityToUpdate)
        {
            return await _profileService.ChangeProfile(entityToUpdate);
        }
    }
}
