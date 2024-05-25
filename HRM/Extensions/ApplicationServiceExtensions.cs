using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace HRM.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            #region Add db context
            services.AddDbContext<HRMContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("HRMConnectionString"));
            });
            #endregion


            return services;
        }
    }
}
