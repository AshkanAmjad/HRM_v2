using AutoMapper;
using Data.Context;
using Data.Extensions;
using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.Role;
using Domain.DTOs.Security.User;
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
        public List<DisplayAssistantsVM> GetAssistants()
        {
            var context = GetAssistantsQuery();
            var assistants = (from item in context
                              where(item.IsActived)
                              orderby item.RegisterDate descending
                              select new DisplayAssistantsVM
                              {
                                  RoleId = item.RoleId,
                                  Title = item.Title,
                                  RegisterDate = $"{item.RegisterDate.ToShamsi()}",
                                  IsActived = (item.IsActived) ? "فعال" : "غیرفعال"
                              })
                              .ToList();
            return assistants;
        }

        public IQueryable<Role> GetAssistantsQuery()
        {
            return _context.Roles.IgnoreQueryFilters()
                                 .AsQueryable();
        }

        public bool RegisterAssistant(AssistantRegisterVM model, out string message)
        {
            string checkMessage = "";
            if (!Similarity(model, out checkMessage))
            {
                model.RegisterDate = DateTime.Now;
                model.IsActived = true;
                model.RoleId = Guid.NewGuid();

                UploadRegisterAssistantToDb(model);

                message = "";
                return true;
            }
            message = checkMessage;
            return false;
        }

        public bool Similarity(AssistantRegisterVM model, out string message)
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


        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UploadEditAssistantToDb(AssistantEditVM model)
        {
            if(model != null)
            {
                Role initial = _context.Roles
                                         .Find(model.RoleId);
                if(initial != null)
                {
                    Role assistant = _mapper.Map<Role>(model);
                    
                    initial.Title = assistant.Title;
                    initial.RegisterDate = assistant.RegisterDate;
                    
                    _context.Update(initial);
                }

            }
        }

        public void UploadRegisterAssistantToDb(AssistantRegisterVM model)
        {
            if(model != null)
            {
                Role assistant = _mapper.Map<Role>(model);
                _context.Add(assistant);
            }
        }
        public void ActiveAssistantDb(AssistantEdit_Active_DisableVM model)
        {
            if (model != null)
            {
                var assistant = _context.Roles.IgnoreQueryFilters()
                                              .Where(r => r.RoleId == model.RoleId)
                                              .FirstOrDefault();

                if (assistant != null)
                {
                    assistant.IsActived = true;
                    assistant.RegisterDate = DateTime.Now;

                    _context.Roles.Update(assistant);
                }
            }
        }

        public bool DisableAssistant(AssistantEdit_Active_DisableVM model, out string message)
        {
            string checkMessage = "عملیات غیر فعال سازی با شکست مواجه شد.";

            if (model != null)
            {

                DisableAssistantDb(model);

                message = "";

                return true;

            }
            message = checkMessage;
            return false;
        }

        public void DisableAssistantDb(AssistantEdit_Active_DisableVM model)
        {
            if (model != null)
            {
                var assistant = _context.Roles
                                        .Find(model.RoleId);

                if (assistant != null)
                {
                    assistant.IsActived = false;
                    assistant.RegisterDate = DateTime.Now;

                    _context.Roles.Update(assistant);
                }
            }
        }

        public bool EditAssistant(AssistantEditVM model, out string message)
        {
            string checkMessage = "اطلاعات ناقص ارسال شده است.";

            if (model != null)
            {
                UploadEditAssistantToDb(model);

                message = "";
                return true;
            }

            message = checkMessage;
            return false;
        }

        public AssistantEditVM? GetAssistantById(Guid roleId)
        {
            AssistantEditVM assistant = new();
            if (roleId != Guid.Empty)
            {
                assistant = (from item in _context.Roles
                             where (item.RoleId == roleId)
                             select new AssistantEditVM
                             {
                                 RoleId = item.RoleId,
                                 Title = item.Title,
                             }).Single();
            }
            return assistant;
        }

        public bool ActiveAssistant(AssistantEdit_Active_DisableVM model, out string message)
        {
            string checkMessage = "عملیات فعال سازی با شکست مواجه شد.";

            if (model != null)
            {
                ActiveAssistantDb(model);

                message = "";
                return true;
            }

            message = checkMessage;
            return false;
        }
    }
}
