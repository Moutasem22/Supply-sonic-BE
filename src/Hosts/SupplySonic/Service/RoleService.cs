
using Core.Models.Identity;
using DB;
using DTO;
using Helpers;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Mapster;
using Microsoft.Extensions.Localization;
using Localization;
using Core.Enums;
using IServiceContractor.ICommonService;
using DocumentFormat.OpenXml.Office.CustomUI;

namespace Service
{
    public class RoleService : IRoleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string lang { get; set; }
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly IBaseService _baseService;

        public RoleService(IBaseService baseService, IHttpContextAccessor httpContextAccessor, IStringLocalizer<SharedResource> _localizer)
        {
            this._baseService = baseService;
            _httpContextAccessor = httpContextAccessor;
            lang = _httpContextAccessor.HttpContext.Request.Headers["lang"].ToString() ?? "ar";
            this.localizer = _localizer;

            TypeAdapterConfig<Role, RoleResultDto>.NewConfig()
            .AfterMapping((src, dest) =>
            {
                dest.PermissionsIds = src.Permissions != null ? src.Permissions.Select(s=>s.PageActionId).ToList() : null;
            });
        }

        public ResultViewModel<List<RoleResultDto>> GetAll(QueryViewModel<RoleResultDto> queryViewModel)
        {
            var PagedDataResult = new ResultViewModel<List<RoleResultDto>>();


            //Expression<Func<Role, bool>> myVar = r => r.IsActive == true && r.IsDeleted != true;

            var query = _baseService.Context.Roles.Where(r => r.IsActive == true && r.IsDeleted != true && !r.IsMaster).Include(x => x.Permissions) as IEnumerable<Role>;

            queryViewModel.Filter.ToList().ForEach(x =>
            {
                switch (x.Operation)
                {
                    case (FilterOperation.Equal):
                        if (x.FieldName.ToLower() == "rolename")
                        {
                            query = query.Where(r => r.Name.Contains(x.value));
                        }
                        break;
                }
            });

            if (queryViewModel.Order.SortType == SortTypeEnum.ASC)
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("name"):
                        query = query.OrderBy(x => x.NameAr);
                        break;
                    default:
                        query = query.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("name"):
                        query = query.OrderByDescending(x => x.NameAr);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.Id);
                        break;
                }
            }

            var Total = query.Count();

            var roles = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

            List<RoleResultDto> roleDtos = roles.Adapt<List<RoleResultDto>>();//Mapper.Map<List<RoleResultDto>>(roles);

            PagedDataResult.Data = roleDtos;
            PagedDataResult.PageSize = queryViewModel.PageSize;
            PagedDataResult.PageNumber = queryViewModel.PageNumber;
            PagedDataResult.Total = Total;
            PagedDataResult.IsSuccess = true;
            return PagedDataResult;

        }

        public ResultViewModel<List<RoleResultDto>> GetAllExceptMaster(QueryViewModel<RoleResultDto> queryViewModel)
        {
            var PagedDataResult = new ResultViewModel<List<RoleResultDto>>();

            var query = _baseService.Context.Roles.Where(r => r.IsActive == true && r.IsDeleted != true && r.IsMaster != true && r.IsAdmin != true).Include(x => x.Permissions) as IEnumerable<Role>;

            queryViewModel.Filter.ToList().ForEach(x =>
            {
                switch (x.Operation)
                {
                    case (FilterOperation.Equal):
                        if (x.FieldName.ToLower() == "rolename")
                        {
                            query = query.Where(r => r.Name.Contains(x.value) || r.NameAr.Contains(x.value));
                        }
                        break;
                }
            });

            if (queryViewModel.Order.SortType == SortTypeEnum.ASC)
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("name"):
                        query = query.OrderBy(x => x.Name);
                        break;
                    case ("namear"):
                        query = query.OrderBy(x => x.NameAr);
                        break;
                    default:
                        query = query.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("name"):
                        query = query.OrderByDescending(x => x.Name);
                        break;
                    case ("namear"):
                        query = query.OrderByDescending(x => x.NameAr);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.Id);
                        break;
                }
            }

            var Total = query.Count();

            var roles = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

            List<RoleResultDto> roleDtos = roles.Adapt<List<RoleResultDto>>();// Mapper.Map<List<RoleResultDto>>(roles);

            PagedDataResult.Data = roleDtos;
            PagedDataResult.PageSize = queryViewModel.PageSize;
            PagedDataResult.PageNumber = queryViewModel.PageNumber;
            PagedDataResult.Total = Total;
            PagedDataResult.IsSuccess = true;
            return PagedDataResult;


        }

        public ResultViewModel<RoleResultDto> Getone(int id)
        {
            var PagedDataResult = new ResultViewModel<RoleResultDto>();
            var query = _baseService.Context.Roles.Where(r => r.Id == id).Include(x => x.Permissions).FirstOrDefault();
            RoleResultDto roleDtos = query.Adapt<RoleResultDto>(); //Mapper.Map<RoleResultDto>(query);

            PagedDataResult.Data = roleDtos;
            PagedDataResult.IsSuccess = true;
            return PagedDataResult;
        }

        public ResultViewModel<List<RoleResultDto>> GetAllByPageId(int id)
        {
            var PagedDataResult = new ResultViewModel<List<RoleResultDto>>();
            var query = _baseService.Context.Roles.AsNoTracking().Include(s => s.UserRoles).ThenInclude(s => s.AppUser).Include(x => x.Permissions.Where(s => s.PageAction.PageId == id)).ThenInclude(s => s.PageAction).Where(s=>s.UserRoles.Count() != 0).ToList();
            List<RoleResultDto> roleDtos = query.Adapt<List<RoleResultDto>>();
            

            foreach (var item in query)
            {
                roleDtos.FirstOrDefault(s => s.Id == item.Id).UserName = item.UserRoles.FirstOrDefault().AppUser.FullName;
                //roleDtos.FirstOrDefault(s => s.Id == item.Id).PermissionsIds=item.Permissions.Select(s => s.PageActionId).ToList();
            }

            PagedDataResult.Data = roleDtos;
            PagedDataResult.IsSuccess = true;
            return PagedDataResult;
        }

        public ResultViewModel<RoleResultDto> Add(RoleAddEditDto roleDto)
        {
            ResultViewModel<RoleResultDto> _ResultViewModel = new ResultViewModel<RoleResultDto>();

            var results = new List<ValidationResult>();
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(roleDto, null, null);
            var isValid = Validator.TryValidateObject(roleDto, context, results, true);
            var Cresult = this.Validate(roleDto);
            results.AddRange(Cresult);

            if (!isValid || results.Count > 0)
            {
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
            }


            var role = new Role(roleDto.NameAr, roleDto.Name, roleDto.PageActions.Select(s => new Permission() { PageActionId = (int)s.Id }).ToList());

            _baseService.Context.Roles.Add(role);
            _baseService.Context.SaveChanges();



            _ResultViewModel.Data = role.Adapt<RoleResultDto>();
            _ResultViewModel.IsSuccess = true;
            return _ResultViewModel;



        }
        public ResultViewModel<RoleResultDto> Update(RoleAddEditDto roleDto)
        {
            ResultViewModel<RoleResultDto> _ResultViewModel = new ResultViewModel<RoleResultDto>();


            var results = new List<ValidationResult>();
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(roleDto, null, null);
            var isValid = Validator.TryValidateObject(roleDto, context, results, true);
            var Cresult = this.Validate(roleDto);
            results.AddRange(Cresult);
            var role = _baseService.Context.Roles.Include(s => s.Permissions).FirstOrDefault(x => x.Id == roleDto.Id);
            if (!StructuralComparisons.StructuralEqualityComparer.Equals(role.RowVersion, roleDto.RowVersion))
            {
                results.Add(new ValidationResult(localizer["RowVersionErr"], new List<string>() { nameof(roleDto.RowVersion) }));
            }
            if (!isValid || results.Count > 0)
            {
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
            }

            role.Update(roleDto.NameAr, roleDto.Name, roleDto.PageActions.Select(s => new Permission() { PageActionId = (int)s.Id }).ToList());
            _baseService.Context.SaveChanges();

            _ResultViewModel.Data = role.Adapt<RoleResultDto>();
            _ResultViewModel.IsSuccess = true;
            return _ResultViewModel;

        }
        public ResultViewModel<RoleResultDto> Delete(int id)
        {
            ResultViewModel<RoleResultDto> _ResultViewModel = new ResultViewModel<RoleResultDto>();


            var results = new List<ValidationResult>();

            var role = _baseService.Context.Roles.FirstOrDefault(x => x.Id == id);
            if (role == null)
            {
                results.Add(new ValidationResult(localizer["ItemNotExist"], new List<string>() { "Role" }));
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
            }
            var userexists = _baseService.Context.UserRoles.Where(x => x.RoleId == id && x.AppUser.IsDeleted != true).Count();
            if (userexists > 0)
            {
                results.Add(new ValidationResult(localizer["ThereIsUsersRelatedToRole"], new List<string>() { "Role" }));
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
            }
            role.Delete();
            _baseService.Context.SaveChanges();

            _baseService.sendActionNotification(EnumPageCode.Role, EnumActionCode.delete, role.Id);

            _ResultViewModel.Data = role.Adapt<RoleResultDto>();
            _ResultViewModel.IsSuccess = true;
            return _ResultViewModel;


        }

        private List<ValidationResult> Validate(RoleAddEditDto model)
        {
            var result = new List<ValidationResult>();

            var exist = _baseService.Context.Roles.FirstOrDefault(x => (x.Name == model.Name || x.NameAr == model.NameAr || x.Name == model.NameAr || x.NameAr == model.Name) && (x.Id != model.Id || model.Id == 0) && x.IsDeleted == false);
            if (exist != null)
            {
                result.Add(new ValidationResult("NameExist", new List<string>() { nameof(model.Name) }));
            }
            if (model.Id != 0)//update mode
            {
                var role = _baseService.Context.Roles.Include(x => x.Permissions).FirstOrDefault(x => x.Id == model.Id);
                if (role == null)
                {
                    result.Add(new ValidationResult("RecordNotExist", new List<string>() { nameof(model.Id) }));
                }
            }

            return result;

        }

    }
}
