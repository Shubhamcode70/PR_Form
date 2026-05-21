using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PRChecksheetApp.Controllers;
using PRChecksheetApp.Models;
using PRChecksheetApp.Services.Interfaces;

namespace PRChecksheetApp.Tests;

public class PurchaseRequisitionControllerTests
{
    [Fact]
    public async Task Submit_ShouldReturnView_WhenCapexFieldsMissing()
    {
        var service = new Mock<IPRService>();
        var env = new Mock<IWebHostEnvironment>();
        env.SetupGet(e => e.WebRootPath).Returns(Path.GetTempPath());
        var controller = new PurchaseRequisitionController(service.Object, env.Object);

        var vm = BuildValidVm();
        vm.Header.PRType = "CAPEX";
        vm.Header.CRID = null;
        vm.Header.AssetNumber = null;

        var result = await controller.Submit(vm, null);

        result.Should().BeOfType<ViewResult>();
        controller.ModelState.ContainsKey("Header.CRID").Should().BeTrue();
        controller.ModelState.ContainsKey("Header.AssetNumber").Should().BeTrue();
        service.Verify(x => x.SavePRAsync(It.IsAny<PurchaseRequisitionVM>()), Times.Never);
    }

    [Fact]
    public async Task Submit_ShouldRedirect_WhenValid()
    {
        var service = new Mock<IPRService>();
        var env = new Mock<IWebHostEnvironment>();
        env.SetupGet(e => e.WebRootPath).Returns(Path.GetTempPath());
        var controller = new PurchaseRequisitionController(service.Object, env.Object);

        var result = await controller.Submit(BuildValidVm(), null);

        result.Should().BeOfType<RedirectToActionResult>();
        service.Verify(x => x.SavePRAsync(It.IsAny<PurchaseRequisitionVM>()), Times.Once);
    }

    [Fact]
    public async Task Submit_ShouldReturnView_WhenMoreThan50Items()
    {
        var service = new Mock<IPRService>();
        var env = new Mock<IWebHostEnvironment>();
        env.SetupGet(e => e.WebRootPath).Returns(Path.GetTempPath());
        var controller = new PurchaseRequisitionController(service.Object, env.Object);

        var vm = BuildValidVm();
        vm.Items = Enumerable.Range(0, 51).Select(_ => BuildValidItem()).ToList();

        var result = await controller.Submit(vm, null);

        result.Should().BeOfType<ViewResult>();
        service.Verify(x => x.SavePRAsync(It.IsAny<PurchaseRequisitionVM>()), Times.Never);
    }

    private static PurchaseRequisitionVM BuildValidVm() => new()
    {
        Header = new PurchaseRequisitionHeader
        {
            RequirementReceivedFrom = "User",
            DepartmentLocation = "HQ",
            PurposeOfProcurement = "Purpose",
            SingleVendorJustification = "Justification",
            PRType = "OPEX"
        },
        Items = [BuildValidItem()]
    };

    private static PurchaseRequisitionItem BuildValidItem() => new()
    {
        ItemNo = "10",
        ShortText = "Item",
        UnitOfMeasure = "EA",
        Quantity = 1,
        ValuationPrice = 10,
        TotalValue = 10,
        DeliveryDate = DateTime.Today,
        MaterialGroup = "MG",
        PlantCode = "P1",
        PurchasingGroup = "PG",
        Requisitioner = "Req",
        QuantityAccountAssignment = 1,
        CostCentre = "CC",
        GLAccount = "GL",
        CostCentreBearer = "Bearer"
    };
}
