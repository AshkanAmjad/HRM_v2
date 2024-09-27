using Domain.DTOs.Security.Role;
using Domain.DTOs.Security.User;
using FluentValidation;

namespace HRM.Models.Validation.Security.Role
{
    public class AssistantRegisterValidator : AbstractValidator<AssistantRegisterVM>
    {
        public AssistantRegisterValidator()
        {
            RuleFor(x => x.Title).NotNull()
                                 .WithMessage("تکمیل ورودی عنوان ضروری است.")
                                 .Matches(@"^[\u0600-\u06FF\s]+$")
                                 .WithMessage("مقدار ورودی  عنوان معتبر نیست.");
        }
    }
}
