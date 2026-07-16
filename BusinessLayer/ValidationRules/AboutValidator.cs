using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class AboutValidator : AbstractValidator<About>
    {
        public AboutValidator()
        {
            RuleFor(x => x.AboutDetails1).NotEmpty().WithMessage("Birinci paragrafı boş geçemezsiniz");
            RuleFor(x => x.AboutDetails1).MinimumLength(20).WithMessage("Birinci paragraf en az 20 karakter olmalıdır");
        }
    }
}