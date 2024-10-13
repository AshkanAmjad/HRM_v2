using Domain.DTOs.Security.Login;
using FluentValidation;

namespace HRM.Models.Validation.Security.Login
{
    public class VerificationCodeValidator : AbstractValidator<VerificationCodeVM>
    {
        public VerificationCodeValidator()
        {
            RuleFor(x => x.Code).NotNull()
                                .NotEmpty()
                                .WithMessage("تکمیل ورودی کد تایید ضروری است.");
        }
    }
}
