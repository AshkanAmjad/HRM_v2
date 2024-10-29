using Application.Extensions;
using Application.Services.Implrmentations;
using Application.Services.Interfaces;
using Data.Repositores;
using Domain.DTOs.Security.Login;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace IOC.Dependencies
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IGeneralService, GeneralService>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<ITransferService, TransferService>();
            #endregion

            #region Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IDepartmentTransferRepository, DepartmentTransferRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            #endregion

            #region General
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
            #endregion
        }
    }
}
