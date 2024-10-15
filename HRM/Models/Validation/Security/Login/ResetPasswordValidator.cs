using Domain.DTOs.Security.Login;
using FluentValidation;

namespace HRM.Models.Validation.Security.Login
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordVM>
    {
        public ResetPasswordValidator()
        {

            RuleFor(x => x.Password).NotNull()
                                   .WithMessage("تکمیل ورودی گذر واژه ضروری است.")
                                   .MaximumLength(15)
                                   .WithMessage("حداکثر تعداد حروف گذر واژه 15 حرف می باشد.");

            RuleFor(x => x.ConfirmPassword).NotNull()
                                           .WithMessage("تکمیل ورودی  تکرار گذر واژه ضروری است.")
                                           .Equal(x => x.Password)
                                           .WithMessage("تکرار گذر واژه با گذر واژه مطابق نمی باشد.");
        }
    }
}
