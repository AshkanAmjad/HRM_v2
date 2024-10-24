using Domain.DTOs.Portal.Document;
using Domain.DTOs.Security.User;
using FluentValidation;

namespace HRM.Models.Validation.Portal.Document
{
    public class DownloadValidator : AbstractValidator<DownloadVM>
    {
        public DownloadValidator()
        {
           RuleFor(d => d.DocumentId).NotNull()
                                     .NotEmpty()
                                     .NotEqual(Guid.Empty);
        }
    }
}
