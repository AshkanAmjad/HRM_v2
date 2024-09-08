using Domain.DTOs.Security.User;
using FluentValidation;

namespace HRM.Models.Validation
{
    public class UserEdit_DeleteValidator: AbstractValidator<UserEdit_DeleteVM>
    {
        public UserEdit_DeleteValidator()
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
