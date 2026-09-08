using Core.Models;
using Core.Models.Products;
using DB;
using DTO;
using DTO.Product;
using Helpers;
using IServiceContractor;
using IServiceContractor.ICommonService;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class BindingRoomService : IBindingRoomService
{
    private readonly IBaseService _baseService;

    private readonly DBContext _dBContext;

    public BindingRoomService(IBaseService baseService)
    {
        _baseService = baseService;
        //_validator = validator;
        _dBContext = baseService.Context;
        TypeAdapterConfig<BindingRoom, BindingRoomResultDto>.NewConfig()
         .Map(dest => dest.ProductDescription, src => _baseService.Language == "ar" ? src.MainDescriptionAr : src.MainDescriptionEn)
         .Map(dest => dest.CreatorName, src => src.SupplierAppUser.FullName)
                 .Map(dest => dest.ProductName, src => _baseService.Language == "ar" ? src.ProductNameAr : src.ProductNameEn)
                 .Map(dest => dest.BindingRoomAttachments, src => src.BindingRoomAttachments.Select(s => s.Attachment).Adapt<List<AttachmentDto>>())

        ;

        TypeAdapterConfig<BindingRoomAttribute, ProductAttribueAddEditDto>.NewConfig()
        .Map(dest => dest.AttributeName, src => src.Attribute != null ? _baseService.Language == "ar" ? src.Attribute.NameAr : src.Attribute.NameEn : "")
        .Map(dest => dest.SubAttributeName, src => src.SubAttribute != null ? _baseService.Language == "ar" ? src.SubAttribute.NameAr : src.SubAttribute.NameEn : "");

        TypeAdapterConfig<Attachment, AttachmentDto>.NewConfig()
         .Map(dest => dest.FilePath, src => _baseService.CurrentUrlPath + src.Path);
        TypeAdapterConfig<BindingRoomRequest, BindingRoomRequestResultDto>.NewConfig()
             //.Map(dest => dest.BindingRoom, src => src.BindingRoom.Adapt<BindingRoom>())
             .Map(dest => dest.CreatorName, src => src.SupplierAppUser.FullName)
             .Map(dest => dest.OrderDate, src => src.CreatedDate.ToISOString())
             ;


    }
    public async Task<ResultViewModel<BindingRoomResultDto>> Add(BindingRoomResultDto lockupDto)
    {
        BindingRoom attribute = new(lockupDto.SupplierAppUserId, lockupDto.ProductNameEn, lockupDto.ProductNameAr, lockupDto.ProductMainCategoryId
            , lockupDto.ProductSubCategoryId, lockupDto.UnitPrice, lockupDto.MainDescriptionEn, lockupDto.MainDescriptionAr, lockupDto.Qaunt,
            lockupDto.UOMId, lockupDto.IsNegotiate, lockupDto.IsSupplier, lockupDto.Status, lockupDto.StartDate, lockupDto.BindingRoomStatus,
            lockupDto.BindingRoomAttributes.Select(item => new BindingRoomAttribute(item.AttributeId, item.SubAttributeId)).ToList()
            , lockupDto.BindingRoomAttachmentsIds.Select(item => new BindingRoomAttachment((int)item)).ToList());
        await _dBContext.BindingRooms.AddAsync(attribute);
        await _dBContext.SaveChangesAsync();


        ResultViewModel<BindingRoomResultDto> result = new() { Data = attribute.Adapt<BindingRoomResultDto>(), IsSuccess = true };
        return result;
    }



    public async Task<ResultViewModel<BindingRoomResultDto>> Delete(int id)
    {
        var attribute = _dBContext.BindingRooms.FirstOrDefault(n => n.Id == id && !n.IsDeleted && n.IsActive);
        if (attribute == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "BindingRoom" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        attribute?.Delete();
        await _dBContext.SaveChangesAsync();

        ResultViewModel<BindingRoomResultDto> result = new() { Data = attribute.Adapt<BindingRoomResultDto>(), IsSuccess = true };
        return result;

    }

    public async Task<ResultViewModel<List<BindingRoomResultDto>>> GetAll(NewQueryViewModel<BindingRoomFilterDto> queryViewModel)
    {
        IQueryable<BindingRoom> query = _dBContext.BindingRooms.Include(s => s.SupplierAppUser).Include(s => s.BindingRoomAttributes).ThenInclude(s => s.Attribute)
                                                                .Include(s => s.BindingRoomAttributes).ThenInclude(s => s.SubAttribute)
                                                               .Include(s => s.BindingRoomAttachments).ThenInclude(s => s.Attachment)
                                     .AsNoTracking().OrderByDescending(x => x.CreatedDate)
                                     .WhereByIf(queryViewModel.SearchModel?.SupplierAppUserId != null,
                                      p => p.SupplierAppUserId == queryViewModel.SearchModel.SupplierAppUserId)

                                      .WhereByIf(queryViewModel.SearchModel?.ProductDescription != null,
                     p =>
                      p.MainDescriptionAr.ToLower().Contains(queryViewModel.SearchModel.ProductDescription.Trim().ToLower())
                    || p.MainDescriptionEn.ToLower().Contains(queryViewModel.SearchModel.ProductDescription.Trim().ToLower()))

                .WhereByIf(queryViewModel.SearchModel?.ProductName != null,
                  p =>
                       p.ProductNameAr.ToLower().Contains(queryViewModel.SearchModel.ProductName.Trim().ToLower())
                     || p.ProductNameEn.ToLower().Contains(queryViewModel.SearchModel.ProductName.Trim().ToLower())
                 )

               .WhereByIf(queryViewModel.SearchModel?.IsSupplier != null, p => (p.IsSupplier == queryViewModel.SearchModel.IsSupplier))


                .WhereByIf(queryViewModel.SearchModel?.Status != null, p => (p.Status == queryViewModel.SearchModel.Status))

               .WhereByIf(queryViewModel.SearchModel?.BindingRoomStatus != null, p => (p.BindingRoomStatus == queryViewModel.SearchModel.BindingRoomStatus))

               .WhereByIf(queryViewModel.SearchModel?.ProductMainCategoryId != null, p => (p.ProductMainCategoryId == queryViewModel.SearchModel.ProductMainCategoryId))

               .WhereByIf(queryViewModel.SearchModel?.ProductSubCategoryId != null, p => (p.ProductSubCategoryId == queryViewModel.SearchModel.ProductSubCategoryId))

               .WhereByIf(queryViewModel.SearchModel?.AttributesIds != null ,
                p => (p.BindingRoomAttributes.Any(s => queryViewModel.SearchModel.AttributesIds.Contains(s.SubAttributeId))))

               .WhereByIf(queryViewModel.SearchModel?.QauntFrom != null && queryViewModel.SearchModel?.QauntTo != null,
                p => (p.Qaunt >= queryViewModel.SearchModel.QauntFrom && p.Qaunt <= queryViewModel.SearchModel.QauntTo));

        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
        var add = lockup.Adapt<List<BindingRoomResultDto>>();
        ResultViewModel<List<BindingRoomResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<BindingRoomResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;


    }



    public async Task<ResultViewModel<BindingRoomResultDto>> Getone(int id)
    {
        var query = await _dBContext.BindingRooms.Include(s => s.SupplierAppUser).
            Include(s => s.BindingRoomAttributes).ThenInclude(s => s.Attribute)
                                                                .Include(s => s.BindingRoomAttributes).ThenInclude(s => s.SubAttribute)
                                                               .Include(s => s.BindingRoomAttachments).ThenInclude(s => s.Attachment)
                                                               .AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        if (query == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        ResultViewModel<BindingRoomResultDto> result = new() { Data = query.Adapt<BindingRoomResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<BindingRoomResultDto>> Update(BindingRoomResultDto lockupDto)
    {
        BindingRoom? attribute = await _dBContext.BindingRooms.Include(s => s.BindingRoomAttachments)
             .Include(s => s.BindingRoomAttributes)

            .FirstOrDefaultAsync(n => n.Id == lockupDto.Id && !n.IsDeleted && n.IsActive);
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


        attribute.Update(lockupDto.SupplierAppUserId, lockupDto.ProductNameEn, lockupDto.ProductNameAr, lockupDto.ProductMainCategoryId
                        , lockupDto.ProductSubCategoryId, lockupDto.UnitPrice, lockupDto.MainDescriptionEn, lockupDto.MainDescriptionAr, lockupDto.Qaunt,
                         lockupDto.UOMId, lockupDto.IsNegotiate, lockupDto.IsSupplier, lockupDto.Status, lockupDto.StartDate, lockupDto.BindingRoomStatus,
                         lockupDto.BindingRoomAttributes.Select(item => new BindingRoomAttribute(item.AttributeId, item.SubAttributeId)).ToList()
            , lockupDto.BindingRoomAttachmentsIds.Select(item => new BindingRoomAttachment((int)item)).ToList());

        await _dBContext.SaveChangesAsync();

        ResultViewModel<BindingRoomResultDto> result = new() { Data = attribute.Adapt<BindingRoomResultDto>(), IsSuccess = true };
        return result;
    }



    #region Binding Room Requests
    public async Task<ResultViewModel<BindingRoomRequestResultDto>> CreateRequest(BindingRoomRequestResultDto lockupDto)
    {
        BindingRoomRequest lockup = new(lockupDto.BindingRoomId, lockupDto.SupplierAppUserId, lockupDto.IsNegotiate);
        await _dBContext.BindingRoomRequests.AddAsync(lockup);
        await _dBContext.SaveChangesAsync();

        if (lockup.IsNegotiate != true)
        {
            var bind = _dBContext.BindingRooms.FirstOrDefault(s => s.Id == lockupDto.BindingRoomId);
            bind.UpdateBindingRoomStatus(Core.Enums.EnumBindingRoomStatus.Claim);
            _dBContext.SaveChanges();
        }

        ResultViewModel<BindingRoomRequestResultDto> result = new() { Data = lockup.Adapt<BindingRoomRequestResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<BindingRoomRequestResultDto>> UpdateRequest(BindingRoomRequestResultDto lockupDto)
    {
        BindingRoomRequest? lockup = await _dBContext.BindingRoomRequests.Include(s => s.BindingRoom)
                                    .FirstOrDefaultAsync(n => n.Id == lockupDto.Id && !n.IsDeleted && n.IsActive);
        if (lockup == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ObjectNotFound", PropertyName = "Project" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (!StructuralComparisons.StructuralEqualityComparer.Equals(lockup.RowVersion, lockupDto.RowVersion))
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "RowVersionErr", PropertyName = "RowVersion" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }


        lockup.Update(lockupDto.BindingRoomId, lockupDto.SupplierAppUserId, lockupDto.IsNegotiate, lockup.BindingRoom);

        await _dBContext.SaveChangesAsync();

        ResultViewModel<BindingRoomRequestResultDto> result = new() { Data = lockup.Adapt<BindingRoomRequestResultDto>(), IsSuccess = true };
        return result;
    }


    public async Task<ResultViewModel<List<BindingRoomRequestResultDto>>> GetAllRequest(NewQueryViewModel<BindingRoomFilterDto> queryViewModel)
    {
        IQueryable<BindingRoomRequest> query = _dBContext.BindingRoomRequests.Include(s => s.SupplierAppUser).Include(s => s.BindingRoom)
                                                   .ThenInclude(s => s.SupplierAppUser).Include(s => s.BindingRoom)
                                                   .ThenInclude(s => s.BindingRoomAttributes).ThenInclude(s => s.Attribute)
                                                   .Include(s => s.BindingRoom).ThenInclude(s => s.BindingRoomAttributes).ThenInclude(s => s.SubAttribute)
                                                  .Include(s => s.BindingRoom).ThenInclude(s => s.BindingRoomAttachments).ThenInclude(s => s.Attachment)
                                     .AsNoTracking().OrderByDescending(x => x.CreatedDate)
                                     .Where(s => s.BindingRoom.BindingRoomStatus == Core.Enums.EnumBindingRoomStatus.NotClaim
                                     )
                                     .WhereByIf(queryViewModel.SearchModel?.SupplierAppUserId != null,
                                      p => p.SupplierAppUserId == queryViewModel.SearchModel.SupplierAppUserId)

                                      .WhereByIf(queryViewModel.SearchModel?.ProductDescription != null,
                     p =>
                      p.BindingRoom.MainDescriptionAr.ToLower().Contains(queryViewModel.SearchModel.ProductDescription.Trim().ToLower())
                    || p.BindingRoom.MainDescriptionEn.ToLower().Contains(queryViewModel.SearchModel.ProductDescription.Trim().ToLower()))

                .WhereByIf(queryViewModel.SearchModel?.ProductName != null,
                  p =>
                       p.BindingRoom.ProductNameAr.ToLower().Contains(queryViewModel.SearchModel.ProductName.Trim().ToLower())
                     || p.BindingRoom.ProductNameEn.ToLower().Contains(queryViewModel.SearchModel.ProductName.Trim().ToLower())
                 )

               .WhereByIf(queryViewModel.SearchModel?.IsSupplier != null, p => (p.BindingRoom.IsSupplier == queryViewModel.SearchModel.IsSupplier))

                .WhereByIf(queryViewModel.SearchModel?.Status != null, p => (p.BindingRoom.Status == queryViewModel.SearchModel.Status))

                .WhereByIf(queryViewModel.SearchModel?.BindingRoomStatus != null, p => (p.BindingRoom.BindingRoomStatus == queryViewModel.SearchModel.BindingRoomStatus))

                .WhereByIf(queryViewModel.SearchModel?.ProductMainCategoryId != null, p => (p.BindingRoom.ProductMainCategoryId == queryViewModel.SearchModel.ProductMainCategoryId))

               .WhereByIf(queryViewModel.SearchModel?.ProductSubCategoryId != null, p => (p.BindingRoom.ProductSubCategoryId == queryViewModel.SearchModel.ProductSubCategoryId))

               .WhereByIf(queryViewModel.SearchModel?.AttributesIds != null,
                  p => (p.BindingRoom.BindingRoomAttributes.Any(s => queryViewModel.SearchModel.AttributesIds.Contains(s.SubAttributeId))))

               .WhereByIf(queryViewModel.SearchModel?.QauntFrom != null && queryViewModel.SearchModel?.QauntTo != null,
                 p => (p.BindingRoom.Qaunt >= queryViewModel.SearchModel.QauntFrom && p.BindingRoom.Qaunt <= queryViewModel.SearchModel.QauntTo));

        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
        ResultViewModel<List<BindingRoomRequestResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<BindingRoomRequestResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;


    }

    public async Task<ResultViewModel<BindingRoomRequestResultDto>> GetOneRequest(int Id)
    {
        var query = await _dBContext.BindingRoomRequests.Include(s => s.SupplierAppUser)
                                  .Include(s => s.BindingRoom).ThenInclude(s => s.SupplierAppUser)
                                  .Include(s => s.BindingRoom).ThenInclude(s => s.BindingRoomAttributes).ThenInclude(s => s.Attribute)
                                  .Include(s => s.BindingRoom).ThenInclude(s => s.BindingRoomAttributes).ThenInclude(s => s.SubAttribute)
                                  .Include(s => s.BindingRoom).ThenInclude(s => s.BindingRoomAttachments).ThenInclude(s => s.Attachment)
                                     .AsNoTracking().FirstOrDefaultAsync(s => s.Id == Id);


        ResultViewModel<BindingRoomRequestResultDto> PagedDataResult = new()
        {
            Data = query.Adapt<BindingRoomRequestResultDto>(),

            IsSuccess = true
        };
        return PagedDataResult;


    }


    #endregion
}


