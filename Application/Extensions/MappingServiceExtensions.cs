using Data.HRMProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class MappingServiceExtensions
    {
        public static IServiceCollection AddMappingService(this IServiceCollection services)
        {
            #region Add Auto Mapping
            services.AddAutoMapper(typeof(UserProfile));
            services.AddAutoMapper(typeof(DocumentProfle));
            #endregion

            return services;
        }
    }
}
