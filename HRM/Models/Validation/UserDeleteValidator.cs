using Domain.DTOs.Security.User;
using FluentValidation;

namespace HRM.Models.Validation
{
    public class UserDeleteValidator: AbstractValidator<UserDeleteVM>
    {
        public UserDeleteValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                                  .NotNull()
                                  .NotEqual(Guid.Empty);

            RuleFor(x => x.UserName).NotNull();

            RuleFor(x => x.Province).NotNull();

            RuleFor(x => x.County).NotNull();

            RuleFor(x => x.District).NotNull();
        }
    }
}
