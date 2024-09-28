using Domain.DTOs.Security.Role;
using Domain.DTOs.Security.UserRole;
using FluentValidation;

namespace HRM.Models.Validation.Security.UserRole
{
    public class UserRoleRegisterValidator : AbstractValidator<UserRoleRegisterVM>
    {
        public UserRoleRegisterValidator()
        {
            RuleFor(x => x.RoleId).NotEqual("معاونت مورد نظر را انتخاب کنید ...")
                                  .WithMessage("تکمیل ورودی معاونت ضروری است.")
                                  .NotEqual($"{Guid.Empty}")
                                  .NotNull()
                                  .NotEmpty();

            RuleFor(x => x.UserId).NotEqual("کاربر مورد نظر را انتخاب کنید ...")
                                  .WithMessage("تکمیل ورودی نام کاربری ضروری است.")
                                  .NotEqual($"{Guid.Empty}")
                                  .NotNull()
                                  .NotEmpty();
        }
    }
}
