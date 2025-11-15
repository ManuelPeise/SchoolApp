using Logic.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Shared
{
    public static class RegisterSharedServices
    {
        public static void RegisterServices(this IServiceCollection services)
        {
           services.AddScoped<ILogManager, LogManager>();
        }
    }
}
