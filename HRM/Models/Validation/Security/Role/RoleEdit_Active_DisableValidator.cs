using Domain.DTOs.Security.Role;
using FluentValidation;

namespace HRM.Models.Validation.Security.Role
{
    public class RoleEdit_Active_DisableValidator:AbstractValidator<RoleEdit_Active_DisableVM>
    {
        public RoleEdit_Active_DisableValidator()
        {
            RuleFor(x => x.RoleId).NotNull()
                                 .NotEmpty()
                                 .NotEqual(Guid.Empty);

            RuleFor(x => x.Title).NotNull()
                                 .NotEmpty();
        }


    }
}
