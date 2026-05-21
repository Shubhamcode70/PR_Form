using PRChecksheetApp.Models;

namespace PRChecksheetApp.Repositories.Interfaces;

public interface IPRRepository
{
    Task AddGraphAsync(PurchaseRequisitionHeader header);
    Task<long> GetNextPrSequenceAsync();
}
