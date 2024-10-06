using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public class AreaPermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private IPermissionRepository _permissionRepository;
        private string _area = "";

        public AreaPermissionCheckerAttribute(string area)
        {
            _area = area;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _permissionRepository=(IPermissionRepository) context.HttpContext.RequestServices.GetService(typeof(IPermissionRepository));


            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string id = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId").Value;

                Guid userId = new Guid(id);

                if (!_permissionRepository.CheckAreaPermission(_area,userId))
                {
                    context.Result = new RedirectResult("/");
                }
            }
            else
            {
                context.Result = new RedirectResult("/");
            }
        }
    }
}
