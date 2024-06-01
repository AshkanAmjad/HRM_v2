using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class MappingServiceExtensions
    {
        public static IServiceCollection AddMappingService(this IServiceCollection services)
        {
            #region Add Auto Mapping
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            return services;
        }
    }
}
