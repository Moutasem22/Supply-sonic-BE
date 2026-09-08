using DTO.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators;

public class ProductOfferValidator : AbstractValidator<ProductOfferAddEditDto>
{
    public ProductOfferValidator()
    {
                
    }
}
