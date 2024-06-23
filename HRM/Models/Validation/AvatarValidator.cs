using FluentValidation;

namespace HRM.Models.Validation
{
    public class AvatarValidator : AbstractValidator<IFormFile>
    {
        public AvatarValidator() {
            RuleFor(x => x.Length).LessThanOrEqualTo(500000)
                                  .WithMessage("حجم تصویر بیش از مقدار مجاز است.");

            RuleFor(x => x.ContentType).Must(x => x.Equals("image/jpeg") || x.Equals("image/png"))
                                       .WithMessage("نوع تصویر ورودی نامعتبر است.");
        }
    }
}
