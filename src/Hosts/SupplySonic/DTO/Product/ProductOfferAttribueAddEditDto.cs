using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product;

public class ProductOfferAttribueAddEditDto
{

    public int AttributeId { get; set; }
    public string? AttributeName { get; set; }
    public int SubAttributeId { get; set; }
    public string? SubAttributeName { get; set; }
}
