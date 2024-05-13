using Application.Services.Implrmentations;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.Dependencies
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Services
            services.AddScoped<IManagementServices,ManagementServices>();
            #endregion

            #region Repository

            #endregion
        }
    }
}
