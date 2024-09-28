using Domain.DTOs.Security.UserRole;
using FluentValidation;

namespace HRM.Models.Validation.Security.UserRole
{
    public class UserRoleEdit_Active_DisableValidator : AbstractValidator<UserRoleEdit_Active_DisableVM>
    {
        public UserRoleEdit_Active_DisableValidator()
        {
            RuleFor(x => x.UserRoleId).NotEqual(Guid.Empty)
                                      .NotNull()
                                      .NotEmpty();
        }

    }
}
