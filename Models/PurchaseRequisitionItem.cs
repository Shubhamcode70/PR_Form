using System.ComponentModel.DataAnnotations;

namespace PRChecksheetApp.Models;

public class PurchaseRequisitionItem
{
    public int Id { get; set; }
    public int HeaderId { get; set; }
    public PurchaseRequisitionHeader? Header { get; set; }

    [Required, StringLength(50)] public string ItemNo { get; set; } = string.Empty;
    [Required] public string ShortText { get; set; } = string.Empty;
    [Required, StringLength(50)] public string UnitOfMeasure { get; set; } = string.Empty;
    [Required] public decimal Quantity { get; set; }
    [Required] public decimal ValuationPrice { get; set; }
    [Required] public decimal TotalValue { get; set; }
    [Required] public DateTime DeliveryDate { get; set; }
    [Required, StringLength(255)] public string MaterialGroup { get; set; } = string.Empty;
    [Required, StringLength(100)] public string PlantCode { get; set; } = string.Empty;
    [Required, StringLength(100)] public string PurchasingGroup { get; set; } = string.Empty;
    [Required, StringLength(255)] public string Requisitioner { get; set; } = string.Empty;
    [Required] public decimal QuantityAccountAssignment { get; set; }
    [Required, StringLength(100)] public string CostCentre { get; set; } = string.Empty;
    [Required, StringLength(100)] public string GLAccount { get; set; } = string.Empty;
    [Required, StringLength(100)] public string CostCentreBearer { get; set; } = string.Empty;
}
