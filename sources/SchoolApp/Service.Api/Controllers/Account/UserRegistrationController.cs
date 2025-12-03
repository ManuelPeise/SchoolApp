using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Service.Api.Controllers.Account
{
    public class UserRegistrationController: ApiControllerBase
    {
        private readonly IUserAdministrationService _userAdministrationService;

        public UserRegistrationController(IUserAdministrationService userAdministrationService)
        {
            _userAdministrationService = userAdministrationService;
        }

        //api/account/registeruser
        [HttpPost(Name = "RegisterUser")]
        public async Task<ResponseBaseModel> RegisterUser([FromBody] UserRegistrationRequestModel model)
        {
            return await _userAdministrationService.RegisterUser(model);
        }
    }
}
