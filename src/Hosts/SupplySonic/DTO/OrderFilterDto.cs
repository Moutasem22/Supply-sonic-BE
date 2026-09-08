using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class OrderFilterDto
{
    public EnumOrderStatus? OrderStatus { get; set; }
    public string? OrderCode { get; set; }
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
}
