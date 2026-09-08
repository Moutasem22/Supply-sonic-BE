using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.RequestDto;

public class RequestProductFilterDTO
{
    public int? ProductMainCategoryId { get; set; }

    public int? ProductSubCategoryId { get; set; }
    public string? ProductName { get; set; }

    public string? ProductDescription { get; set; }

    public List<int>? AttributesIds { get; set; }

    public int? UOMFrom { get; set; }

    public int? UOMTo { get; set; }
}
