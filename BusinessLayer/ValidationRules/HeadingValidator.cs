using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class HeadingValidator : AbstractValidator<Heading>
    {
        public HeadingValidator()
        {
            RuleFor(x => x.HeadingName).NotEmpty().WithMessage("Başlık adını boş geçemezsiniz");
            RuleFor(x => x.HeadingName).MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalıdır");
            RuleFor(x => x.HeadingName).MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir");
            RuleFor(x => x.CategoryID).GreaterThan(0).WithMessage("Kategori seçmelisiniz");
        }
    }
}