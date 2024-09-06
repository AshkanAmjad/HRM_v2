using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace HRM.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, WebApplicationBuilder builder, IConfiguration configuration)
        {
            #region DB Context
            services.AddDbContext<HRMContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("HRMConnectionString"));
            });
            #endregion

            return services;
        }
    }
}
