using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Service.Api.Controllers.Health
{
    public class AvailabilityController: ApiControllerBase
    {
        public AvailabilityController()
        {
                
        }

        [HttpGet(Name = "IsAvailable")]
        public async Task<ResponseBaseModel> IsAvailable()
        {
            return await Task.FromResult(new ResponseBaseModel
            {
                Success = true,
                Message = "Service is available"
            });
        }
    }
}
