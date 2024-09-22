using Domain.DTOs.Security.User;
using FluentValidation;

namespace HRM.Models.Validation.Security
{
    public class UserDelete_ActiveValidator: AbstractValidator<UserDelete_ActiveVM>
    {
        public UserDelete_ActiveValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
                                  .NotNull()
                                  .NotEqual(Guid.Empty);

            RuleFor(x => x.Area).NotNull();

            RuleFor(x => x.Province).NotNull();

            RuleFor(x => x.County).NotNull();

            RuleFor(x => x.District).NotNull();

        }
    }
}
