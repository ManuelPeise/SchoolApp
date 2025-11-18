using Data.Entities;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Service.Api.Controllers.Account
{
    public class UserRegistrationController: ApiControllerBase
    {
        private readonly IUserAdministrationService _userAdministrationService;

        public UserRegistrationController(IUserAdministrationService userAdministrationService)
        {
            _userAdministrationService = userAdministrationService;
        }

        [HttpPost(Name = "RegisterUser")]
        public async Task<DatabaseModel?> RegisterUser([FromBody] UserRegistrationRequestModel model)
        {
            return await _userAdministrationService.RegisterUser(model);
        }
    }
}
