﻿using Domain.DTOs.Security.User;
using FluentValidation;

namespace HRM.Models.Validation
{
    public class UserRegisterValidator:AbstractValidator<UserRegisterVM>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.AreaDepartment).NotNull();

            RuleFor(x=>x.Avatar).SetValidator(new AvatarValidator());

            RuleFor(x => x.UserName).Length(10)
                                    .WithMessage("تعداد ارقام کدملی باید 10 رقم باشد.")
                                    .NotNull()
                                    .WithMessage("تکمیل ورودی کد ملی ضروری است.");

            RuleFor(x => x.Password).NotNull()
                                    .WithMessage("تکمیل ورودی گذر واژه ضروری است.")
                                    .MaximumLength(15)
                                    .WithMessage("حداکثر تعداد حروف گذر واژه 15 حرف می باشد.");

            RuleFor(x => x.ConfirmPassword).NotNull()
                                           .WithMessage("تکمیل ورودی  تکرار گذر واژه ضروری است.")
                                           .Equal(x => x.Password)
                                           .WithMessage("تکرار گذر واژه با گذر واژه مطابق نمی باشد.");

            RuleFor(x => x.FirstName).NotNull()
                                     .WithMessage("تکمیل ورودی نام ضروری است.")
                                     .Matches(@"^[\u0600-\u06FF\s]+$")
                                     .WithMessage("مقدار ورودی  نام معتبر نیست.");
           
            RuleFor(x => x.LastName).NotNull()
                                    .WithMessage("تکمیل ورودی نام خانوادگی ضروری است.")
                                    .Matches(@"^[\u0600-\u06FF\s]+$")
                                    .WithMessage("مقدار ورودی نام خانوادگی معتبر نیست.");

            RuleFor(x => x.Education).NotEqual("سطح تحصیلات خود را انتخاب کنید ...")
                                     .WithMessage("تکمیل ورودی تحصیلات ضروری است.");

            RuleFor(x => x.Gender).NotNull()
                                  .WithMessage("تکمیل ورودی جنسیت ضروری است.");
                
            RuleFor(x => x.MaritalStatus).NotNull()
                                         .WithMessage("تکمیل ورودی وضعیت تاهل ضروری است.");

            RuleFor(x => x.EmploymentStatus).NotEqual("وضعیت استخدامی خود را انتخاب کنید")
                                            .WithMessage("تکمیل ورودی وضعیت استخدامی ضروری است.");

            RuleFor(x => x.ProvinceDepartment).NotEqual("شعبه سطح استان خود را انتخاب کنید ...")
                                    .WithMessage("تکمیل ورودی شعبه استان ضروری است.");

            RuleFor(x => x.CountyDepartment).NotEqual("شعبه سطح شهرستان خود را انتخاب کنید ...")
                                  .WithMessage("تکمیل ورودی شعبه شهرستان ضروری است.");

            RuleFor(x => x.DistrictDepartment).NotEqual("شعبه سطح بخش خود را انتخاب کنید ...")
                                    .WithMessage("تکمیل ورودی شعبه بخش ضروری است.");

            RuleFor(x => x.PhoneNumber).NotNull()
                                       .WithMessage("تکمیل ورودی شماره تماس همراه ضروری است.")
                                       .Length(11)
                                       .WithMessage("تعداد ارقام شماره تماس همراه باید 11 رقم باشد.");

            RuleFor(x => x.Email).NotNull()
                                 .WithMessage("تکمیل ورودی پست الکترونیک ضروری است.")
                                 .EmailAddress()
                                 .WithMessage("مقدار ورودی پست الکرونیک معتبر نیست.");

            RuleFor(x => x.DateOfBirth).NotNull()
                                       .WithMessage("تکمیل ورودی تاریخ تولد ضروری است.");

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
