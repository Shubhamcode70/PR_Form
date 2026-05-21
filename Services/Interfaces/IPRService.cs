using PRChecksheetApp.Models;

namespace PRChecksheetApp.Services.Interfaces;

public interface IPRService
{
    Task SavePRAsync(PurchaseRequisitionVM vm);
}
