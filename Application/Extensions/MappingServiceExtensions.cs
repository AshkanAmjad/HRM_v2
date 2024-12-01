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
            services.AddAutoMapper(typeof(DepartmentTransferProfile));
            services.AddAutoMapper(typeof(ReportProfile));
            services.AddAutoMapper(typeof(RoleProfile));
            services.AddAutoMapper(typeof(TransferProfile));
            services.AddAutoMapper(typeof(UserRoleProfile));
            #endregion

            return services;
        }
    }
}
