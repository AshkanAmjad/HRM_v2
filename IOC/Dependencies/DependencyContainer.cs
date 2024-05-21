using Application.Services.Implrmentations;
using Application.Services.Interfaces;
using Domain.DTOs.Security.Login;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace IOC.Dependencies
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Services
            services.AddScoped<IUserService,UserService>();
            #endregion



            #region Repository

            #endregion
        }
    }
}
