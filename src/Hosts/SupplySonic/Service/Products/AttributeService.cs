using Core.Models.Products;
using DB;
using DTO;
using Helpers;
using IServiceContractor.ICommonService;
using IServiceContractor.Products;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = Core.Models.Products.Attribute;

namespace Service.Products
{
    public class AttributeService : IAttributeService
    {
        private readonly IBaseService _baseService;
        
        private readonly DBContext _dBContext;

        public AttributeService(IBaseService baseService)
        {
            _baseService = baseService;
            //_validator = validator;
            _dBContext = baseService.Context;
            TypeAdapterConfig<Attribute, LockupResultDto>.NewConfig()
                 .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn);
            TypeAdapterConfig<Attribute, ChildLockupResultDto>.NewConfig()
             .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn);
        }
        public async Task<ResultViewModel<LockupResultDto>> Add(LockupResultDto lockupDto)
        {
            Attribute attribute = new (lockupDto.NameEn, lockupDto.NameAr);
            await _dBContext.Attributes.AddAsync(attribute);
            await _dBContext.SaveChangesAsync();


            ResultViewModel<LockupResultDto> result = new() { Data = attribute.Adapt<LockupResultDto>(), IsSuccess = true };
            return result;
        }

        public async Task<ResultViewModel<LockupResultDto>> Delete(int id)
        {
            var attribute = _dBContext.Attributes.FirstOrDefault(n => n.Id == id && !n.IsDeleted && n.IsActive);
            if (attribute == null)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "Attribute" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }
            attribute?.Delete();
            await _dBContext.SaveChangesAsync();

            ResultViewModel<LockupResultDto> result = new() { Data = attribute.Adapt<LockupResultDto>(), IsSuccess = true };
            return result;

        }

        public async Task<ResultViewModel<List<LockupResultDto>>> GetAll(NewQueryViewModel<LockupResultDto> queryViewModel)
        {
            IQueryable<Attribute> query = _dBContext.Attributes.AsNoTracking().OrderByDescending(x => x.Id)
                                         .WhereByIf(!string.IsNullOrEmpty(queryViewModel.SearchModel?.Name),
                                           p => (p.NameAr.ToLower().Contains(queryViewModel.SearchModel.Name.Trim().ToLower())
                                               || p.NameEn.ToLower().Contains(queryViewModel.SearchModel.Name.Trim().ToLower())))

                                             .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameAr), x => x.NameAr, queryViewModel.SortBy?.NameAr)
                                             .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameEn), x => x.NameEn, queryViewModel.SortBy?.NameEn);

            var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
            ResultViewModel<List<LockupResultDto>> PagedDataResult = new()
            {
                Data = lockup.Adapt<List<LockupResultDto>>(),
                PageSize = queryViewModel.PageSize,
                PageNumber = queryViewModel.PageNumber,
                Total = query.Count(),
                IsSuccess = true
            };
            return PagedDataResult;


        }

        public async Task<ResultViewModel<List<LockupResultDto>>> GetAllWithSubs(QueryViewModel<LockupResultDto> queryViewModel)
        {
            IQueryable<Attribute> query = _dBContext.Attributes.AsNoTracking()
           .Include(s => s.SubAttributes).Where(n => !n.IsDeleted && n.IsActive);
            var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
            ResultViewModel<List<LockupResultDto>> PagedDataResult = new()
            {
                Data = lockup.Adapt<List<LockupResultDto>>(),
                PageSize = queryViewModel.PageSize,
                PageNumber = queryViewModel.PageNumber,
                Total = query.Count(),
                IsSuccess = true
            };
            return PagedDataResult;
        }

        public async Task<ResultViewModel<LockupResultDto>> Getone(int id)
        {
            var query = await _dBContext.Attributes.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

            if (query == null)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }
            ResultViewModel<LockupResultDto> result = new() { Data = query.Adapt<LockupResultDto>(), IsSuccess = true };
            return result;
        }

        public async Task<ResultViewModel<LockupResultDto>> Update(LockupResultDto lockupDto)
        {
            Attribute? attribute = await _dBContext.Attributes.FirstOrDefaultAsync(n => n.Id == lockupDto.Id && !n.IsDeleted && n.IsActive);
            if (attribute == null)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ObjectNotFound", PropertyName = "Project" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }

            if (!StructuralComparisons.StructuralEqualityComparer.Equals(attribute.RowVersion, lockupDto.RowVersion))
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "RowVersionErr", PropertyName = "RowVersion" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }
            attribute.Update(lockupDto.NameEn, lockupDto.NameAr);
            await _dBContext.SaveChangesAsync();

            ResultViewModel<LockupResultDto> result = new() { Data = attribute.Adapt<LockupResultDto>(), IsSuccess = true };
            return result;
        }
    }
}
