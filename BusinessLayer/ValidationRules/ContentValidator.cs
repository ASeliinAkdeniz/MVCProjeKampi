using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class ContentValidator : AbstractValidator<Content>
    {
        public ContentValidator()
        {
            RuleFor(x => x.ContentValue).NotEmpty().WithMessage("Entry boş olamaz");
            RuleFor(x => x.ContentValue).MinimumLength(5).WithMessage("Entry en az 5 karakter olmalıdır");
            RuleFor(x => x.ContentValue).MaximumLength(2000).WithMessage("Entry en fazla 2000 karakter olabilir");
        }
    }
}