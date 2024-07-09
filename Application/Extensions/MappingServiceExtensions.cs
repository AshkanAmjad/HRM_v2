using Data.HRMProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Extensions
{
    public static class MappingServiceExtensions
    {
        public static IServiceCollection AddMappingService(this IServiceCollection services)
        {
            #region Add Auto Mapping
            services.AddAutoMapper(typeof(UserProfile));
            #endregion
            return services;
        }
    }
}
