using Domain.DTOs.General;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.Role;
using Domain.DTOs.Security.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using HRM.Models.Validation;
using HRM.Models.Validation.Security;
using HRM.Models.Validation.Security.Role;
using HRM.Models.Validation.Security.User;

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
            services.AddScoped<IValidator<UserEdit_DisableVM>, UserEdit_DisableValidator>();
            services.AddScoped<IValidator<UserEditVM>,UserEditValidator>();
            services.AddScoped<IValidator<AreaVM>,AreaValidator>();
            services.AddScoped<IValidator<UserDelete_ActiveVM>, UserDelete_ActiveValidator>();

            services.AddScoped<IValidator<AssistantRegisterVM>, AssistantRegisterValidator>();
            services.AddScoped<IValidator<AssistantEditVM>, AssistantEditValidator>();
            services.AddScoped<IValidator<AssistantEdit_Active_DisableVM>, AssistantEdit_DisableValidator>();
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
