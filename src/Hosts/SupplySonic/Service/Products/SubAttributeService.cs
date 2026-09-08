using Core.Models.Products;
using DB;
using DTO;
using Helpers;
using IServiceContractor.ICommonService;
using IServiceContractor.Products;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Products
{
    public class SubAttributeService : ISubAttributeService
    {
        private readonly IBaseService _baseService;
  
        private readonly DBContext _dBContext;
        public SubAttributeService(IBaseService baseService
           
            )
        {
            _baseService = baseService;
            
            _dBContext = baseService.Context;
            TypeAdapterConfig<SubAttribute, ChildLockupResultDto>.NewConfig()
            .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn)
            .Map(dest => dest.ParentId, src => src.AttributeId)
            .Map(dest => dest.ParentName, src => src.Attribute != null ? _baseService.Language == "ar" ? src.Attribute.NameAr : src.Attribute.NameEn : "");
        }

        public async Task<ResultViewModel<ChildLockupResultDto>> Add(ChildLockupResultDto lockupDto)
        {
            SubAttribute subAttribute = new(lockupDto.NameEn, lockupDto.NameAr, lockupDto.ParentId);
            await _dBContext.SubAttributes.AddAsync(subAttribute);
            await _dBContext.SaveChangesAsync();

            ResultViewModel<ChildLockupResultDto> result = new() { Data = null, IsSuccess = true };
            return result;
        }

        public async Task<ResultViewModel<ChildLockupResultDto>> Delete(int id)
        {
            var subAttribute = _dBContext.SubAttributes.FirstOrDefault(n => n.Id == id && !n.IsDeleted && n.IsActive);
            if (subAttribute == null)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "SubAttribute" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }

            subAttribute?.Delete();
            await _dBContext.SaveChangesAsync();

            ResultViewModel<ChildLockupResultDto> result = new() { Data = subAttribute.Adapt<ChildLockupResultDto>(), IsSuccess = true };
            return result;
        }

        public async Task<ResultViewModel<List<ChildLockupResultDto>>> GetAll(NewQueryViewModel<ChildLockupResultDto> queryViewModel)
        {
            IQueryable<SubAttribute> query = _dBContext.SubAttributes.Include(s => s.Attribute).AsNoTracking().OrderByDescending(x => x.Id)
                                         .WhereByIf(!string.IsNullOrEmpty(queryViewModel.SearchModel?.Name),
                                           p => (p.NameAr.ToLower().Contains(queryViewModel.SearchModel.Name.Trim().ToLower())
                                               || p.NameEn.ToLower().Contains(queryViewModel.SearchModel.Name.Trim().ToLower())))

                                             .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameAr), x => x.NameAr, queryViewModel.SortBy?.NameAr)
                                             .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameEn), x => x.NameEn, queryViewModel.SortBy?.NameEn);

            var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
            ResultViewModel<List<ChildLockupResultDto>> PagedDataResult = new()
            {
                Data = lockup.Adapt<List<ChildLockupResultDto>>(),
                PageSize = queryViewModel.PageSize,
                PageNumber = queryViewModel.PageNumber,
                Total = query.Count(),
                IsSuccess = true
            };
            return PagedDataResult;

        }

        public async Task<ResultViewModel<List<ChildLockupResultDto>>> GetAllByAttributeId(int? attributeId)
        {
            IQueryable<SubAttribute> query =  _dBContext.SubAttributes.Include(s => s.Attribute)

           .AsNoTracking().Where(n => !n.IsDeleted && n.IsActive & n.AttributeId == attributeId);

            var lockup = query.ToList();
            ResultViewModel<List<ChildLockupResultDto>> PagedDataResult = new()
            {
                Data = lockup.Adapt<List<ChildLockupResultDto>>(),
                Total = query.Count(),
                IsSuccess = true
            };
            return PagedDataResult;
        }

        public async Task<ResultViewModel<ChildLockupResultDto>> Getone(int id)
        {
            var query = await _dBContext.SubAttributes.Include(s => s.Attribute).AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

            if (query == null)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }
            ResultViewModel<ChildLockupResultDto> result = new() { Data = query.Adapt<ChildLockupResultDto>(), IsSuccess = true };
            return result;
        }

        public async Task<ResultViewModel<ChildLockupResultDto>> Update(ChildLockupResultDto lockupDto)
        {
            SubAttribute? subAttribute = await _dBContext.SubAttributes.FirstOrDefaultAsync(n => n.Id == lockupDto.Id && !n.IsDeleted && n.IsActive);
            if (subAttribute == null)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ObjectNotFound", PropertyName = "Project" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(subAttribute?.RowVersion, lockupDto.RowVersion))
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "RowVersionErr", PropertyName = "RowVersion" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }
            subAttribute.Update(lockupDto.NameEn, lockupDto.NameAr, lockupDto.ParentId);
            await _dBContext.SaveChangesAsync();

            ResultViewModel<ChildLockupResultDto> result = new() { Data = subAttribute.Adapt<ChildLockupResultDto>(), IsSuccess = true };
            return result;
        }
    }
}
