using Microsoft.AspNetCore.Mvc;

namespace Service.Api.Controllers.Health
{
    public class HealthCheckController: ApiControllerBase
    {
        public HealthCheckController()
        {
                
        }

        [HttpGet(Name = "CheckHealth")]
        public async Task<bool> CheckHealth()
        {
            return await Task.FromResult(true);
        }
    }
}
