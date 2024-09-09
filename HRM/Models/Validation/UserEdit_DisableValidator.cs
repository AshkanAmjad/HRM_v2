using Domain.DTOs.Security.User;
using FluentValidation;

namespace HRM.Models.Validation
{
    public class UserEdit_DisableValidator: AbstractValidator<UserEdit_DisableVM>
    {
        public UserEdit_DisableValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                                  .NotNull()
                                  .NotEqual(Guid.Empty);

            RuleFor(x => x.Province).NotNull();

            RuleFor(x => x.County).NotNull();

            RuleFor(x => x.District).NotNull();
        }
    }
}
