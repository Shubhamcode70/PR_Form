using System.ComponentModel.DataAnnotations;

namespace PRChecksheetApp.Models;

public class PurchaseRequisitionVM : IValidatableObject
{
    public PurchaseRequisitionHeader Header { get; set; } = new();
    public List<PurchaseRequisitionItem> Items { get; set; } = new() { new PurchaseRequisitionItem { DeliveryDate = DateTime.Today } };

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Items.Count == 0)
        {
            yield return new ValidationResult("At least one line item is required.", [nameof(Items)]);
        }

        if (Items.Count > 50)
        {
            yield return new ValidationResult("A single PR can have a maximum of 50 items.", [nameof(Items)]);
        }
    }
}
