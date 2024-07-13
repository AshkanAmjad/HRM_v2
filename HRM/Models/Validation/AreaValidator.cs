using Domain.DTOs.General;
using FluentValidation;

namespace HRM.Models.Validation
{
    public class AreaValidator: AbstractValidator<AreaVM>
    {
        public AreaValidator()
        {
            RuleFor(x => x.Province).NotNull();

            RuleFor(x => x.County).NotNull();

            RuleFor(x => x.District).NotNull();
        }
    }
}
