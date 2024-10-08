using Domain.DTOs.Security.Profile;
using Domain.DTOs.Security.User;
using FluentValidation;

namespace HRM.Models.Validation.Security.Profile
{
    public class ProfileEditValidator : AbstractValidator<ProfileEditVM>
    {
        public ProfileEditValidator()
        {
            RuleFor(x => x.AreaDepartment).NotNull();

            RuleFor(x => x.CountyDepartment).NotNull();

            RuleFor(x => x.DistrictDepartment).NotNull();

            RuleFor(x => x.Avatar).SetValidator(new AvatarValidator());
            RuleFor(x => x.Password)
                        .MaximumLength(15)
                        .WithMessage("حداکثر تعداد حروف گذر واژه 15 حرف می باشد.");

            RuleFor(x => x.UserId).NotNull()
                      .NotEmpty()
                      .NotEqual(Guid.Empty);

            RuleFor(x => x.FirstName).NotNull();

            RuleFor(x => x.LastName).NotNull();

            RuleFor(x => x.ConfirmPassword)
                               .Equal(x => x.Password)
                               .WithMessage("تکرار گذر واژه با گذر واژه مطابق نمی باشد.");

            RuleFor(x => x.Education).NotEqual("سطح تحصیلات خود را انتخاب کنید ...")
                                     .WithMessage("تکمیل ورودی تحصیلات ضروری است.");

            RuleFor(x => x.MaritalStatus).NotNull()
                             .WithMessage("تکمیل ورودی وضعیت تاهل ضروری است.");

            RuleFor(x => x.PhoneNumber).NotNull()
                                       .WithMessage("تکمیل ورودی شماره تماس همراه ضروری است.")
                                       .Length(11)
                                       .WithMessage("تعداد ارقام شماره تماس همراه باید 11 رقم باشد.");

            RuleFor(x => x.Email).NotNull()
                                 .WithMessage("تکمیل ورودی پست الکترونیک ضروری است.")
                                 .EmailAddress()
                                 .WithMessage("مقدار ورودی پست الکرونیک معتبر نیست.");

            RuleFor(x => x.City).NotNull()
                                .WithMessage("تکمیل ورودی شهر محل سکونت ضروری است.")
                                .Matches(@"^[\u0600-\u06FF\s]+$")
                                .WithMessage("مقدار ورودی شهر سکونت معتبر نیست.");

            RuleFor(x => x.Address).NotNull()
                                   .WithMessage("تکمیل ورودی نشانی محل سکونت ضروری است.")
                                   .Matches(@"^[\u0600-\u06FF\s]+$")
                                   .WithMessage("مقدار ورودی نشانی سکونت معتبر نیست.")
                                   .MinimumLength(10)
                                   .WithMessage("حداقل تعداد حروف نشانی محل سکونت 10 حرف می باشد.");

        }
    }
}
