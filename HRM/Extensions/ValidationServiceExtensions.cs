﻿using Domain.DTOs.General;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Portal.Transfer;
using Domain.DTOs.Security.Login;
using Domain.DTOs.Security.Profile;
using Domain.DTOs.Security.Role;
using Domain.DTOs.Security.User;
using Domain.DTOs.Security.UserRole;
using FluentValidation;
using FluentValidation.AspNetCore;
using HRM.Models.Validation;
using HRM.Models.Validation.General;
using HRM.Models.Validation.Portal.Document;
using HRM.Models.Validation.Portal.Transfer;
using HRM.Models.Validation.Security;
using HRM.Models.Validation.Security.Login;
using HRM.Models.Validation.Security.Profile;
using HRM.Models.Validation.Security.Role;
using HRM.Models.Validation.Security.User;
using HRM.Models.Validation.Security.UserRole;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            services.AddScoped<IValidator<UsernameValidationVM>, UsernameValidationValidator>();
            services.AddScoped<IValidator<VerificationCodeVM>, VerificationCodeValidator>();
            services.AddScoped<IValidator<ResetPasswordVM>, ResetPasswordValidator>();

            services.AddScoped<IValidator<UserRegisterVM>,UserRegisterValidator>();
            services.AddScoped<IValidator<UserEdit_DisableVM>, UserEdit_DisableValidator>();
            services.AddScoped<IValidator<UserEditVM>,UserEditValidator>();
            services.AddScoped<IValidator<AreaVM>,AreaValidator>();
            services.AddScoped<IValidator<UserDelete_ActiveVM>, UserDelete_ActiveValidator>();

            services.AddScoped<IValidator<RoleRegisterVM>, RoleRegisterValidator>();
            services.AddScoped<IValidator<RoleEditVM>, RoleEditValidator>();
            services.AddScoped<IValidator<RoleEdit_Active_DisableVM>, RoleEdit_Active_DisableValidator>();

            services.AddScoped<IValidator<UserRoleRegisterVM>, UserRoleRegisterValidator>();
            services.AddScoped<IValidator<UserRoleEditVM>, UserRoleEditValidator>();
            services.AddScoped<IValidator<UserRoleEdit_Active_DisableVM>, UserRoleEdit_Active_DisableValidator>();

            services.AddScoped<IValidator<ProfileEditVM>, ProfileEditValidator>();

            services.AddScoped<IValidator<DownloadVM>, DownloadValidator>();

            services.AddScoped<IValidator<TransferRegisterVM>,TransferRegisterValidator>();
            services.AddScoped<IValidator<TransferActive_Disable_DescriptionVM>, TransferActive_Disable_DescriptionValidator>();

            #endregion

            #region Authenication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = "";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            });

            #endregion
        }

    }
}
