using DTO;
using FluentValidation;
using IServiceContractor.ICommonService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class ClientValidator : AbstractValidator<ClientAddEditDto>
    {
        private readonly IBaseService baseService;
        public ClientValidator(IBaseService baseService)
        {
            this.baseService = baseService;

            RuleFor(x => x.Id);
            RuleFor(x => x.NameAr).NotNull().WithMessage("NameIsRequired").Must(IsExistName).WithMessage("NameIsExist");
            RuleFor(x => x.NameEn).NotNull().WithMessage("NameIsRequired").Must(IsExistName).WithMessage("NameIsExist");
            RuleFor(x => x.PhoneNumber).NotNull().WithMessage("PhoneNumberIsRequired").Must(IsExistPhoneNumber).WithMessage("PhoneNumberIsExist");
            RuleFor(x => x.Email).NotNull().WithMessage("EmailIsRequired").Must(IsExistEmail).WithMessage("EmailIsExist");
        }
        private bool IsExistName(ClientAddEditDto dto, string name)
        {
            var exist = baseService.Context.Clients.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id
                && (n.NameAr == name || n.NameAr == name || n.NameEn == name || n.NameEn == name));
            return exist != null ? false : true;
        }
        private bool IsExistPhoneNumber(ClientAddEditDto dto, string phoneNumber)
        {

            var exist = baseService.Context.Clients.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id
                 && n.PhoneNumber == phoneNumber);
            return exist != null ? false : true;
        }
        private bool IsExistEmail(ClientAddEditDto dto, string email)
        {

            var exist = baseService.Context.Clients.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id
                 && n.Email == email );
            return exist != null ? false : true;
        }
      
    }
}
