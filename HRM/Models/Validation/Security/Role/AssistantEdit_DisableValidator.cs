using Domain.DTOs.Security.Role;
using FluentValidation;

namespace HRM.Models.Validation.Security.Role
{
    public class AssistantEdit_DisableValidator:AbstractValidator<AssistantEdit_Active_DisableVM>
    {
        public AssistantEdit_DisableValidator()
        {
            RuleFor(x => x.RoleId).NotNull()
                                 .NotEmpty()
                                 .NotEqual(Guid.Empty);

            RuleFor(x => x.Title).NotNull()
                                 .NotEmpty();
        }


    }
}
