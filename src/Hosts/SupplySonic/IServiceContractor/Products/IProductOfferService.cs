using DTO.Product;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.RequestDto;

namespace IServiceContractor.Products;

public interface IProductOfferService
{
    Task<ResultViewModel<ProductOfferResultDto>> Add(ProductOfferAddEditDto lockDto);
    Task<ResultViewModel<ProductOfferResultDto>> Delete(int id);
    Task<ResultViewModel<List<ProductOfferResultDto>>> GetAll(int ProductId, NewQueryViewModel<ProductOfferResultDto> queryViewModel);
    Task<ResultViewModel<ProductOfferResultDto>> Getone(int id);
    Task<ResultViewModel<ProductOfferResultDto>> Update(ProductOfferAddEditDto lockDto);
    Task<ResultViewModel<List<AttachmentDto>>> GetProductOfferAttachments(int id);
    Task<ResultViewModel<List<ProductOfferAttribueAddEditDto>>> GetProductOfferAttribues(int id);

    Task<ResultViewModel<bool>> SaveProductOfferAttribues(int ProductOfferId, List<ProductOfferAttribueAddEditDto> dto);

    Task<ResultViewModel<bool>> SaveProductOfferAttachments(int ProductOfferId, List<int> ids);


    Task<ResultViewModel<List<ProductOfferPriceScheduleResultDto>>> GetProductOfferPriceSchedules(int id);

    Task<ResultViewModel<bool>> SaveProductOfferPriceSchedules(int ProductOfferId, List<ProductOfferPriceScheduleResultDto> dto);

    Task<ResultViewModel<List<ProductViewDto>>> GetAllProductOfferView(NewQueryViewModel<ProductViewDto> queryViewModel);


    Task<ResultViewModel<List<ProductViewDto>>> SearchOffer(NewQueryViewModel<RequestProductFilterDTO> queryViewModel);
}
