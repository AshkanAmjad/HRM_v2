using Domain.DTOs.Portal.Transfer;
using Domain.DTOs.Security.User;
using FluentValidation;

namespace HRM.Models.Validation.Portal.Transfer
{
    public class TransferRegisterValidator : AbstractValidator<TransferRegisterVM>
    {
        public TransferRegisterValidator()
        {
            RuleFor(t => t.Title).NotNull()
                                 .WithMessage("تکمیل ورودی عنوان ضروری است.")
                                 .MaximumLength(100)
                                 .WithMessage("حداکثر تعداد حروف عنوان 100 حرف می باشد.")
                                 .MinimumLength(1)
                                 .WithMessage("حداقل تعداد حروف عنوان 1 حرف می باشد.")
                                 .Matches(@"^[\u0600-\u06FF\s]+$")
                                 .WithMessage("مقدار ورودی عنوان معتبر نیست.");

            RuleFor(t => t.Description).MaximumLength(300)
                                 .WithMessage("حداکثر تعداد حروف توضیحات 300 حرف می باشد.")
                                 .Matches(@"^[\u0600-\u06FF\s]+$")
                                 .WithMessage("مقدار ورودی توضیحات معتبر نیست.");

            RuleFor(t => t.AreaReceiver).NotNull()
                                        .NotEmpty();


        }
    }
}
