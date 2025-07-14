using DTO.SettingDTO;
using FluentValidation;
using IServiceContractor.ICommonService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class AppearanceSettingsValidator : AbstractValidator<AppearanceAddEditDto>
    {
        private readonly IBaseService _baseService;
        public AppearanceSettingsValidator(IBaseService baseService)
        {
            _baseService = baseService;
            RuleFor(a => a.Id).Must((x, name) => { return IsObjectExist(x.Id); }).WithMessage("ObjectNotFound").When(x => x.Id > 0);
            //RuleFor(a => a.NameAr).NotNull().WithMessage("NameIsRequired").Must(IsExistName).WithMessage("NameIsExist"]);
            // RuleFor(a => a.NameEn).NotNull().WithMessage("NameIsRequird").Must(IsExistName).WithMessage("NameIsExist"]);

            RuleFor(a => a).Must((dto, name) => { return CheckRowVersion(dto); }).WithMessage("RowVersionErr").When(x => x.Id > 0);

            //if (lockup == null)
            //    throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(new ValidationResult("ObjectNotFound", new List<string>() { "Title" })));
            //if (!StructuralComparisons.StructuralEqualityComparer.Equals(lockup.RowVersion, lockupDto.RowVersion))
            //    throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(new ValidationResult("RowVersionErr", new List<string>() { nameof(lockupDto.RowVersion) })));
        }
        private bool IsObjectExist(int Id)
        {
            var exist =  _baseService.Context.AppearancSettings.Where(n => n.Id == Id && !n.IsDeleted).FirstOrDefault();
            return exist == null ? false : true;
        }
        private bool CheckRowVersion(AppearanceAddEditDto dto)
        {
            var exist = _baseService.Context.AppearancSettings.Where(n => n.Id == dto.Id && !n.IsDeleted).FirstOrDefault();
            return exist != null ? StructuralComparisons.StructuralEqualityComparer.Equals(exist.RowVersion, dto.RowVersion) : true;
        }
        //private bool IsExistName(TitleAddEditDto dto, string name)
        //{
        //    var exist = _baseService.Context.Titles.FirstOrDefault(a => !a.IsDeleted &&
        //       a.Id != dto.Id && (a.NameAr == name || a.NameAr == name || a.NameEn == name || a.NameEn == name));
        //    return exist != null ? false : true;
        //}
    }
}
