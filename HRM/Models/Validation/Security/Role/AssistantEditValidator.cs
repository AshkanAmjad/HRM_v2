﻿using Domain.DTOs.Security.Role;
using FluentValidation;

namespace HRM.Models.Validation.Security.Role
{
    public class AssistantEditValidator: AbstractValidator<AssistantEditVM>
    {
        public AssistantEditValidator()
        {
            RuleFor(x =>x.RoleId).NotEmpty()
                                 .NotNull()
                                 .NotEqual(Guid.Empty);

            RuleFor(x => x.Title).NotNull()
                                 .WithMessage("تکمیل ورودی عنوان ضروری است.")
                                 .Matches(@"^[\u0600-\u06FF\s]+$")
                                 .WithMessage("مقدار ورودی  عنوان معتبر نیست.");

        }
    }
}
