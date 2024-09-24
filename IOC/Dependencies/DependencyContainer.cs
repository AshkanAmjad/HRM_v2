﻿using Application.Services.Implrmentations;
using Application.Services.Interfaces;
using Data.Repositores;
using Domain.DTOs.Security.Login;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace IOC.Dependencies
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDocumentService, DocumentService>();
            #endregion

            #region Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            #endregion
        }
    }
}
