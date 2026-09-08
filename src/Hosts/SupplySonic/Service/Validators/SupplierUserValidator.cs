using DTO;
using FluentValidation;
using IServiceContractor.ICommonService;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Service.Validators
{
    public class SupplierUserValidator : AbstractValidator<SupplierAppUserAddEditDto>
    {
        private readonly IBaseService baseService;

        public SupplierUserValidator(IBaseService baseService)
        {
            this.baseService = baseService;

            RuleFor(x => x.Id);
            RuleFor(x => x.FullName).NotNull().WithMessage("NameIsRequired").Must(IsExistName).WithMessage("NameIsExist");
            //RuleFor(x => x.UserName).NotNull().WithMessage("UserNameIsRequired").Must(IsExistUserName).WithMessage("UserNameIsExist");
            RuleFor(x => x.Email).NotNull().WithMessage("EmailIsRequired").Must(IsExistEmail).WithMessage("EmailIsExist");

            RuleFor(x => x.Password).NotNull().WithMessage("PasswordIsRequired")
               .MinimumLength(6).WithMessage("ShouldBe6lettersOrMore")
               .Must(hasNumber).WithMessage("ShouldHaveOneNumberAtLeast")
               .Must(hasUpperChar).WithMessage("ShouldHaveOneCapitalLetterAtLeast")
               .Must(hasLowerChar).WithMessage("ShouldHaveOneSmallLetterAtLeast")
               .Must(hasSpicialCharacter).WithMessage("ShouldHaveOneSpicialCharacterAtLeast");
            //RuleFor(x => x.HasLicense).Must(IsLicenseAvaliable).WithMessage("ThereAreNoVacantActiveLicenses");
        }

        private bool IsExistName(SupplierAppUserAddEditDto dto, string name)
        {
            var exist = baseService.Context.SupplierAppUsers.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id && n.FullName == name);
            return exist != null ? false : true;
        }

        private bool IsExistUserName(SupplierAppUserAddEditDto dto, string userName)
        {
            var exist = baseService.Context.SupplierAppUsers.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id && n.UserName == userName);
            return exist != null ? false : true;
        }
        private bool IsExistEmail(SupplierAppUserAddEditDto dto, string email)
        {
            var exist = baseService.Context.SupplierAppUsers.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id && n.Email == email);
            return exist != null ? false : true;
        }
        private bool hasNumber(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            return !hasNumber.IsMatch(password) ? false : true;
        }

        private bool hasUpperChar(string password)
        {
            var hasUpperChar = new Regex(@"[A-Z]+");
            return !hasUpperChar.IsMatch(password) ? false : true;
        }

        private bool hasLowerChar(string password)
        {
            var hasLowerChar = new Regex(@"(?=.*[a-z])");
            return !hasLowerChar.IsMatch(password) ? false : true;
        }
        private bool hasSpicialCharacter(string password)
        {
            var hasSpicialCharacter = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            return !hasSpicialCharacter.IsMatch(password) ? false : true;
        }

    }
}
