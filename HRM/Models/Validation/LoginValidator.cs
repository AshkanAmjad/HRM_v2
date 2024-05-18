using Domain.DTOs.Security.Login;
using FluentValidation;

namespace HRM.Models.Validation
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Area).NotEqual("ناحیه شعبه را انتخاب کنید")
                                .WithMessage("تکمیل ورودی ناحیه شعبه ضروری است.");


            RuleFor(x => x.UserName).Length(10)
                                    .WithMessage("تعداد ارقام کدملی باید 10 رقم باشد.")
                                    .NotNull()
                                    .WithMessage("تکمیل ورودی کد ملی ضروری است.");

            RuleFor(x => x.Password).NotNull()
                                    .WithMessage("تکمیل ورودی گذر واژه ضروری است.")
                                    .MaximumLength(15)
                                    .WithMessage("حداکثر تعداد حروف گذر واژه 15 حرف می باشد.");
        }
    }
}
