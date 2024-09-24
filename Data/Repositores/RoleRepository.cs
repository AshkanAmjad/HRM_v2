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
    }
}
