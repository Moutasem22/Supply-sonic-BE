using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;

public class ProductWeight : BaseEntity<int>
{
    public int ProductId { get; private set; }
    public virtual Product? Product { get; set; }
    //Unites Of Measures 
    public int? UOMId { get; private set; }
    public virtual Unit? UOM { get; set; }
    //Packing Factor 
    public int? PackingUnitId { get; private set; }
    public virtual Unit? PackingUnit { get; set; }
    public double PackingQuantity { get; private set; }
    //Loading Factor 
    public int? LoadingUnitId { get; private set; }
    public virtual Unit? LoadingUnit { get; set; }
    public double LoadingQuantity { get; private set; }

    //Quantities
    public double MOQ { get; private set; }
    public int? MOQUnitId { get; private set; }
    public virtual Unit? MOQUnit { get; set; }

    //Production
    public double ProductionQuantity { get; private set; }
    public int? ProductionUnitId { get; private set; }
    public virtual Unit? ProductionUnit { get; set; }

    public ProductWeight()
    {

    }

    public ProductWeight(int productId, int? uOMId, int? packingUnitId, double packingQuantity,
                          int? loadingUnitId, double loadingQuantity, int? mOQUnitId, double mOQ,
                          double productionQuantity, int? productionUnitId)
    {
        this.ProductId = productId;
        this.UOMId = uOMId;
        this.PackingUnitId = packingUnitId;
        this.PackingQuantity = packingQuantity;
        this.LoadingUnitId = loadingUnitId;
        this.LoadingQuantity = loadingQuantity;
        this.MOQUnitId = mOQUnitId;
        this.MOQ = mOQ;
        this.ProductionQuantity = productionQuantity;
        this.ProductionUnitId = productionUnitId;


    }

    public ProductWeight(int? uOMId, int? packingUnitId, double packingQuantity,
                        int? loadingUnitId, double loadingQuantity, int? mOQUnitId, double mOQ,
                        double productionQuantity, int? productionUnitId)
    {
        this.UOMId = uOMId;
        this.PackingUnitId = packingUnitId;
        this.PackingQuantity = packingQuantity;
        this.LoadingUnitId = loadingUnitId;
        this.LoadingQuantity = loadingQuantity;
        this.MOQUnitId = mOQUnitId;
        this.MOQ = mOQ;
        this.ProductionQuantity = productionQuantity;
        this.ProductionUnitId = productionUnitId;


    }
}
