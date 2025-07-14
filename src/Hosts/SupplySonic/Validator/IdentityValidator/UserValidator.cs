using DTO.IdentityDTO;
using FluentValidation;
using IServiceContractor.ICommonService;
using Microsoft.EntityFrameworkCore;

namespace Validator.IdentityValidator
{
    public class UserValidator : AbstractValidator<UserAddEditDto>
    {
        private readonly IBaseService baseService;

        public UserValidator(IBaseService baseService)
        {
            this.baseService = baseService;

            RuleFor(x => x.Id);
            RuleFor(x => x.UserId).NotNull().WithMessage("UserIdIsRequired").Must(IsExistUserId).WithMessage("UserIdIsExist").MaximumLength(6).WithMessage("MaximumLength6Digits");
            RuleFor(x => x.FullName).NotNull().WithMessage("NameIsRequired").Must(IsExistName).WithMessage("NameIsExist");
            RuleFor(x => x.UserName).NotNull().WithMessage("UserNameIsRequired").Must(IsExistUserName).WithMessage("UserNameIsExist");
            RuleFor(x => x.Email).NotNull().WithMessage("EmailIsRequired").Must(IsExistEmail).WithMessage("EmailIsExist");
            //RuleFor(x => x.HasLicense).Must(IsLicenseAvaliable).WithMessage("ThereAreNoVacantActiveLicenses");
        }

        private bool IsExistUserId(UserAddEditDto dto, string userId)
        {
            var exist = baseService.Context.Users.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id && n.UserId == userId);
            return exist != null ? false : true;
        }

        private bool IsExistName(UserAddEditDto dto, string name)
        {
            var exist = baseService.Context.Users.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id && n.FullName == name);
            return exist != null ? false : true;
        }

        private bool IsExistUserName(UserAddEditDto dto, string userName)
        {
            var exist = baseService.Context.Users.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id && n.UserName == userName);
            return exist != null ? false : true;
        }
        private bool IsExistEmail(UserAddEditDto dto, string email)
        {
            var exist = baseService.Context.Users.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id && n.Email == email);
            return exist != null ? false : true;
        }


        //private bool IsLicenseAvaliable(UserAddEditDto dto,bool? hasLicense)
        //{
        //    if (dto.Id == 0)
        //    {
        //        if (hasLicense == true) //userType != 0
        //        {
        //            var License = baseService.Context.Licenses.FirstOrDefault(x => x.IsActive && !x.IsDeleted);
        //            var LicensedUsers = baseService.Context.Users.Include(x => x.licensedUser).Where(s => (s.licensedUser != null && s.licensedUser.IsActive && !s.licensedUser.IsDeleted) && s.IsActive && !s.IsDeleted).ToList();

        //            if (LicensedUsers.Count >= (License != null ? License.LicensedUsersCount : 0))
        //                return false;
        //            else return true;
        //        }
        //        else return true;
        //    }
        //    else
        //    {
        //        if (hasLicense == true) //userType != 0
        //        { 
        //            var existLicense = baseService.Context.LicensedUsers.FirstOrDefault(x => x.AppUserId == dto.Id && x.IsActive && !x.IsDeleted);
        //            if (existLicense == null) { 
        //            var License = baseService.Context.Licenses.FirstOrDefault(x => x.IsActive && !x.IsDeleted);
        //            var LicensedUsers = baseService.Context.Users.Include(x => x.licensedUser).Where(s => (s.licensedUser != null && s.licensedUser.IsActive && !s.licensedUser.IsDeleted) && s.IsActive && !s.IsDeleted).ToList();

        //            if (LicensedUsers.Count >= (License != null ? License.LicensedUsersCount : 0))
        //            return false;
        //            else return true;
        //            }
        //            else return true;
        //        }
        //        else return true;
        //    }
        //}
    }
}
