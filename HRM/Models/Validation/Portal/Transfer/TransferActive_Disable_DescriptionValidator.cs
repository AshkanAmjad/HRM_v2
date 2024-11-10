using Domain.DTOs.Portal.Transfer;
using FluentValidation;

namespace HRM.Models.Validation.Portal.Transfer
{
    public class TransferActive_Disable_DescriptionValidator : AbstractValidator<TransferActive_Disable_DescriptionVM>
    {
        public TransferActive_Disable_DescriptionValidator()
        {
            RuleFor(t => t.TransferId).NotNull()
                                      .NotEmpty()
                                      .NotEqual(Guid.Empty);
        }
    }
}
