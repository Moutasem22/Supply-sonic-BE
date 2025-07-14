using DTO.IdentityDTO;
using FluentValidation;
using IServiceContractor.ICommonService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.IdentityValidator
{
    public class UserProfileValidator : AbstractValidator<UserProfileEditDto>
    {
        private readonly IBaseService baseService;

        public UserProfileValidator(IBaseService baseService)
        {
            this.baseService = baseService;
            RuleFor(x => x.Id);
            RuleFor(x => x.PhoneNumber).MaximumLength(15).WithMessage("MaximumLength15Digits");
            RuleFor(x => x.Extension).MaximumLength(5).WithMessage("MaximumLength5Digits");
            //.NotNull().WithMessage("PhoneNumberIsRequired")
            //.NotNull().WithMessage("ExtensionIsRequired")
            //RuleFor(x => x.ProfileAttachmentId).NotNull().WithMessage("ProfileAttachmentIsRequired");
        }
    }
}
