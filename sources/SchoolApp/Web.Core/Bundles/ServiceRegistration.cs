using Data.DbAccessLayer;
using Logic.Shared;
using Microsoft.EntityFrameworkCore;

namespace Web.Core.Bundles
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddDbContext<SchoolContext>(opt =>
            {
                var connectionString = configurationManager.GetConnectionString("SchoolContext");

                if (connectionString == null)
                {
                    throw new Exception("SchoolContext not found!");
                }
                
                opt.UseMySQL(connectionString);
            });

            RegisterSharedServices.RegisterServices(services);
        }
    }
}
