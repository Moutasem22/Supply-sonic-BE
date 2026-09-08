using DTO.Product;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor.Products;

public interface IProductService
{
    Task<ResultViewModel<ProductAddEditDto>> Add(ProductAddEditDto lockDto);
    Task<ResultViewModel<ProductResultDto>> Delete(int id);
    Task<ResultViewModel<List<ProductResultDto>>> GetAll(int SupplierId,NewQueryViewModel<ProductResultDto> queryViewModel);
    Task<ResultViewModel<ProductResultDto>> Getone(int id);
    Task<ResultViewModel<ProductResultDto>> Update(ProductAddEditDto lockDto);

    Task<ResultViewModel<List<AttachmentDto>>> GetProductAttachments(int id);
    Task<ResultViewModel<List<ProductAttribueAddEditDto>>> GetProductAttribues(int id);

    Task<ResultViewModel<bool>> SaveProductAttribues(int ProductId,List<ProductAttribueAddEditDto> dto);

    Task<ResultViewModel<bool>> SaveProductAttachments(int ProductId, List<int> ids);


    Task<ResultViewModel<List<ProductWeightResultDto>>> GetProductWeights(int id);

    Task<ResultViewModel<bool>> SaveProductWeights(int ProductId, List<ProductWeightResultDto> dto);


}
