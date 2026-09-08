using DTO;
using FluentValidation;
using IServiceContractor.ICommonService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators;

public class CityValidator : AbstractValidator<CityAddEditDto>
{
    private readonly IBaseService baseService;
    public CityValidator(IBaseService baseService)
    {
        this.baseService = baseService;

        RuleFor(x => x.Id);
        RuleFor(x => x.NameAr).NotNull().WithMessage("NameIsRequired").Must(IsExistName).WithMessage("NameIsExist");
        RuleFor(x => x.NameEn).NotNull().WithMessage("NameIsRequired").Must(IsExistName).WithMessage("NameIsExist");
        RuleFor(x => x.NameRussian).NotNull().WithMessage("NameIsRequired").Must(IsExistName).WithMessage("NameIsExist");
        RuleFor(x => x.NameUzbek).NotNull().WithMessage("NameIsRequired").Must(IsExistName).WithMessage("NameIsExist");
    }
    private bool IsExistName(CityAddEditDto dto, string name)
    {
        var exist = baseService.Context.Cities.AsNoTracking().FirstOrDefault(n => n.IsActive && !n.IsDeleted && n.Id != dto.Id
            && (n.NameAr == name || n.NameAr == name || n.NameRussian == name || n.NameUzbek == name));
        return exist != null ? false : true;
    }


}