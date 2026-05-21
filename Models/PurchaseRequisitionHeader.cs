using System.ComponentModel.DataAnnotations;

namespace PRChecksheetApp.Models;

public class PurchaseRequisitionHeader
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string PRRequestId { get; set; } = string.Empty;

    [Required, StringLength(255)]
    public string RequirementReceivedFrom { get; set; } = string.Empty;

    [Required, StringLength(255)]
    public string DepartmentLocation { get; set; } = string.Empty;

    [Required]
    public string PurposeOfProcurement { get; set; } = string.Empty;

    [Required]
    public string SingleVendorJustification { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string PRType { get; set; } = string.Empty;

    [StringLength(100)]
    public string? CRID { get; set; }

    [StringLength(100)]
    public string? AssetNumber { get; set; }

    [StringLength(500)]
    public string? AttachmentPath { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PurchaseRequisitionItem> Items { get; set; } = new List<PurchaseRequisitionItem>();
}
