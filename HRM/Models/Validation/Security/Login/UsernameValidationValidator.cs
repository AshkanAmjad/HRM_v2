using Domain.DTOs.Security.Login;
using FluentValidation;

namespace HRM.Models.Validation.General
{
    public class UsernameValidationValidator : AbstractValidator<UsernameValidationVM>
    {
        public UsernameValidationValidator() 
        {
            RuleFor(x => x.Area).NotEqual("ناحیه شعبه را انتخاب کنید")
                                    .WithMessage("تکمیل ورودی ناحیه شعبه ضروری است.");


            RuleFor(x => x.UserName).Length(10)
                                    .WithMessage("تعداد ارقام کدملی باید 10 رقم باشد.")
                                    .NotNull()
                                    .WithMessage("تکمیل ورودی کد ملی ضروری است.");
        }
    }
}
