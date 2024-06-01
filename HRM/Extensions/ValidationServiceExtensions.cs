using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using HRM.Models.Validation;

namespace HRM.Extensions
{
    public static class ValidationServiceExtensions
    {
        public static void ValidationServices(this IServiceCollection services)
        {
            #region Services
            services.AddFluentValidationClientsideAdapters();
            #endregion

            #region Life Time
            services.AddScoped<IValidator<LoginVM>, LoginValidator>();
            services.AddScoped<IValidator<UserRegisterVM>,UserRegisterValidator>();
            #endregion

            #region Authenication
            services.AddAuthentication()
                .AddCookie("HRM", options =>
                {
                    options.Cookie.Name = "HRM";
                    options.LoginPath = "/Index";
                    options.LogoutPath = "/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(10);
                });
            
            #endregion
        }

    }
}
