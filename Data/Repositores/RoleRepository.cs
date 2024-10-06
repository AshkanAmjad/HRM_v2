using AutoMapper;
using Data.Context;
using Data.Extensions;
using Domain.DTOs.Security.Role;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositores
{
    public class RoleRepository : IRoleRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IMapper _mapper;
        public RoleRepository(HRMContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion
        public List<DisplayRolesVM> GetRoles()
        {
            var context = GetRolesQuery();
            var roles = (from item in context
                              orderby item.IsActived descending,item.RegisterDate descending
                              select new DisplayRolesVM
                              {
                                  RoleId = item.RoleId,
                                  Title = item.Title,
                                  RegisterDate = $"{item.RegisterDate.ToShamsi()}",
                                  IsActived = (item.IsActived) ? "فعال" : "غیرفعال"
                              })
                              .ToList();
            return roles;
        }



        public IQueryable<Role> GetRolesQuery()
        {
            return _context.Roles.IgnoreQueryFilters()
                                 .AsQueryable();
        }

        public bool RegisterRole(RoleRegisterVM model, out string message)
        {
            string checkMessage = "";
            if (!Similarity(model, out checkMessage))
            {
                model.RegisterDate = DateTime.Now;
                model.IsActived = true;
                model.RoleId = Guid.NewGuid();

                UploadRegisterRoleToDb(model);

                message = "";
                return true;
            }
            message = checkMessage;
            return false;
        }

        public bool Similarity(RoleRegisterVM model, out string message)
        {
            bool check = false;
            var resultMessage = "";
            if (model != null)
            {
                check = _context.Roles.IgnoreQueryFilters()
                                      .Where(r => r.Title == model.Title )
                                      .Any();

                if (check)
                {
                    resultMessage = $"معاونتی با عنوان {model.Title} قبلا ثبت شده است.";
                }
            }
            message = resultMessage;
            return check;
        }

        public bool Similarity(RoleEditVM model, out string message)
        {
            bool check = false;
            var resultMessage = "";
            if (model != null)
            {
                check = _context.Roles.IgnoreQueryFilters()
                                      .Where(r =>r.RoleId != model.RoleId && r.Title == model.Title)
                                      .Any();

                if (check)
                {
                    resultMessage = $"معاونتی با عنوان {model.Title} قبلا ثبت شده است.";
                }
            }
            message = resultMessage;
            return check;
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UploadEditRoleToDb(RoleEditVM model)
        {
            if(model != null)
            {
                Role initial = _context.Roles
                                         .Find(model.RoleId);
                if(initial != null)
                {
                    Role role = _mapper.Map<Role>(model);
                    
                    initial.Title = role.Title;
                    initial.RegisterDate = role.RegisterDate;
                    
                    _context.Update(initial);
                }

            }
        }

        public void UploadRegisterRoleToDb(RoleRegisterVM model)
        {
            if(model != null)
            {
                Role role = _mapper.Map<Role>(model);
                _context.Add(role);
            }
        }
        public void ActiveRoleDb(RoleEdit_Active_DisableVM model)
        {
            if (model != null)
            {
                var role = _context.Roles.IgnoreQueryFilters()
                                              .Where(r => r.RoleId == model.RoleId)
                                              .FirstOrDefault();

                if (role != null)
                {
                    role.IsActived = true;
                    role.RegisterDate = DateTime.Now;

                    _context.Roles.Update(role);
                }
            }
        }

        public bool DisableRole(RoleEdit_Active_DisableVM model, out string message)
        {
            string checkMessage = "عملیات غیر فعال سازی با شکست مواجه شد.";

            if (model != null)
            {

                DisableRoleDb(model);

                message = "";

                return true;

            }
            message = checkMessage;
            return false;
        }

        public void DisableRoleDb(RoleEdit_Active_DisableVM model)
        {
            if (model != null)
            {
                var role = _context.Roles
                                        .Find(model.RoleId);

                if (role != null)
                {
                    role.IsActived = false;
                    role.RegisterDate = DateTime.Now;

                    _context.Roles.Update(role);
                }
            }
        }

        public bool EditRole(RoleEditVM model, out string message)
        {
            string checkMessage = "";

            if (!Similarity(model, out checkMessage))
            {
                model.RegisterDate = DateTime.Now;

                UploadEditRoleToDb(model);

                message = "";
                return true;
            }

            message = checkMessage;
            return false;
        }

        public RoleEditVM? GetRoleById(Guid roleId)
        {
            RoleEditVM role = new();
            if (roleId != Guid.Empty)
            {
                role = (from item in _context.Roles
                             where (item.RoleId == roleId)
                             select new RoleEditVM
                             {
                                 RoleId = item.RoleId,
                                 Title = item.Title,
                             }).Single();
            }
            return role;
        }

        public bool ActiveRole(RoleEdit_Active_DisableVM model, out string message)
        {
            string checkMessage = "عملیات فعال سازی با شکست مواجه شد.";

            if (model != null)
            {
                ActiveRoleDb(model);

                message = "";
                return true;
            }

            message = checkMessage;
            return false;
        }
    }
}
