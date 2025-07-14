using DTO.IdentityDTO;
using FluentValidation;
using IServiceContractor.ICommonService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Validator.IdentityValidator
{
    public class ChangePasswordValidator : AbstractValidator<PasswordDto>
    {

        private readonly IBaseService _baseService;
        public ChangePasswordValidator(IBaseService baseService)
        {
            _baseService = baseService;
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("PasswordIsRequired")
                .MinimumLength(6).WithMessage("ShouldBe6lettersOrMore")
                .Must(hasNumber).WithMessage("ShouldHaveOneNumberAtLeast")
                .Must(hasUpperChar).WithMessage("ShouldHaveOneCapitalLetterAtLeast")
                .Must(hasLowerChar).WithMessage("ShouldHaveOneSmallLetterAtLeast")
                .Must(hasSpicialCharacter).WithMessage("ShouldHaveOneSpicialCharacterAtLeast");

            RuleFor(x => x.OldPassword).NotNull().WithMessage("PasswordIsRequired")
               .MinimumLength(6).WithMessage("ShouldBe6lettersOrMore")
               .Must(hasNumber).WithMessage("ShouldHaveOneNumberAtLeast")
               .Must(hasUpperChar).WithMessage("ShouldHaveOneCapitalLetterAtLeast")
               .Must(hasLowerChar).WithMessage("ShouldHaveOneSmallLetterAtLeast")
               .Must(hasSpicialCharacter).WithMessage("ShouldHaveOneSpicialCharacterAtLeast");

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
