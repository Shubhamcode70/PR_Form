using Xunit;
using FluentAssertions;
using Moq;
using PRChecksheetApp.Models;
using PRChecksheetApp.Repositories.Interfaces;
using PRChecksheetApp.Services;

namespace PRChecksheetApp.Tests;

public class PRServiceTests
{
    [Fact]
    public async Task SavePRAsync_ShouldCalculateTotalsAndSetPRId_AndCallRepository()
    {
        var repo = new Mock<IPRRepository>();
        repo.Setup(x => x.GetNextPrSequenceAsync()).ReturnsAsync(12);

        PurchaseRequisitionHeader? captured = null;
        repo.Setup(x => x.AddGraphAsync(It.IsAny<PurchaseRequisitionHeader>()))
            .Callback<PurchaseRequisitionHeader>(h => captured = h)
            .Returns(Task.CompletedTask);

        var sut = new PRService(repo.Object);
        var vm = new PurchaseRequisitionVM
        {
            Header = new PurchaseRequisitionHeader
            {
                RequirementReceivedFrom = "User",
                DepartmentLocation = "HQ",
                PurposeOfProcurement = "Need",
                SingleVendorJustification = "Justified",
                PRType = "OPEX"
            },
            Items =
            [
                new PurchaseRequisitionItem { Quantity = 2, ValuationPrice = 50 },
                new PurchaseRequisitionItem { Quantity = 1.5m, ValuationPrice = 20 }
            ]
        };

        await sut.SavePRAsync(vm);

        vm.Items[0].TotalValue.Should().Be(100);
        vm.Items[1].TotalValue.Should().Be(30);
        vm.Header.PRRequestId.Should().EndWith("000012");
        captured.Should().NotBeNull();
        captured!.Items.Should().HaveCount(2);
        repo.Verify(x => x.GetNextPrSequenceAsync(), Times.Once);
        repo.Verify(x => x.AddGraphAsync(It.IsAny<PurchaseRequisitionHeader>()), Times.Once);
    }
}
