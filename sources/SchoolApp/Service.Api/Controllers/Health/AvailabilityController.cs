using Microsoft.AspNetCore.Mvc;

namespace Service.Api.Controllers.Health
{
    public class AvailabilityController: ApiControllerBase
    {
        public AvailabilityController()
        {
                
        }

        [HttpGet(Name = "IsAvailable")]
        public async Task<bool> IsAvailable()
        {
            return await Task.FromResult(true);
        }
    }
}
