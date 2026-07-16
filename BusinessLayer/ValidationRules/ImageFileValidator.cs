using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class ImageFileValidator : AbstractValidator<ImageFile>
    {
        public ImageFileValidator()
        {
            RuleFor(x => x.ImageName).NotEmpty().WithMessage("Görsel adı boş olamaz");
            RuleFor(x => x.ImagePath).NotEmpty().WithMessage("Görsel linki boş olamaz");
        }
    }
}