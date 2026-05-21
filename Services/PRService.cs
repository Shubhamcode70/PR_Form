using PRChecksheetApp.Models;
using PRChecksheetApp.Repositories.Interfaces;
using PRChecksheetApp.Services.Interfaces;

namespace PRChecksheetApp.Services;

public class PRService(IPRRepository repository) : IPRService
{
    private readonly IPRRepository _repository = repository;

    public async Task SavePRAsync(PurchaseRequisitionVM vm)
    {
        foreach (var item in vm.Items)
        {
            item.TotalValue = item.Quantity * item.ValuationPrice;
        }

        var next = await _repository.GetNextPrSequenceAsync();
        vm.Header.PRRequestId = $"PR-{DateTime.UtcNow.Year}-{next:D6}";
        vm.Header.CreatedAt = DateTime.UtcNow;
        vm.Header.Items = vm.Items;

        await _repository.AddGraphAsync(vm.Header);
    }
}
