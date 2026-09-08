using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product;

public class ProductWeightResultDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    //Unites Of Measures 
    public int? UOMId { get; set; }
    //Packing Factor 
    public int? PackingUnitId { get; set; }
    public double PackingQuantity { get; set; }
    //Loading Factor 
    public int? LoadingUnitId { get; set; }
    public double LoadingQuantity { get; set; }

    //Quantities
    public double MOQ { get; set; }
    public int? MOQUnitId { get; set; }

    //Production
    public double ProductionQuantity { get; set; }
    public int? ProductionUnitId { get; set; }
}
