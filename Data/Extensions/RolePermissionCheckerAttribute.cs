using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public class RolePermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private IPermissionRepository _permissionRepository;
        private readonly string[] _roles ;
        public RolePermissionCheckerAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _permissionRepository = (IPermissionRepository)context.HttpContext.RequestServices.GetService(typeof(IPermissionRepository));

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string id = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value;

                Guid userId = new Guid(id);

                if (_permissionRepository.CheckRolePermission(_roles, userId))
                {
                    return;
                }
            }

                context.Result = new RedirectResult("/");
            
        }
    }
}
