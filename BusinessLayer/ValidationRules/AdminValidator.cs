using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class AdminValidator : AbstractValidator<Admin>
    {
        public AdminValidator()
        {
            RuleFor(x => x.AdminUserName).NotEmpty().WithMessage("Kullanıcı adı boş olamaz");
            RuleFor(x => x.AdminUserName).EmailAddress().WithMessage("Geçerli bir mail adresi giriniz");
            RuleFor(x => x.AdminRole).NotEmpty().WithMessage("Yetki seçmelisiniz");
        }
    }
}