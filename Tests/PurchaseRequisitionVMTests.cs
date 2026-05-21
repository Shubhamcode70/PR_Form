using Xunit;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using PRChecksheetApp.Models;

namespace PRChecksheetApp.Tests;

public class PurchaseRequisitionVMTests
{
    [Fact]
    public void Validate_ShouldFail_WhenItemsAreMoreThan50()
    {
        var vm = new PurchaseRequisitionVM { Items = Enumerable.Range(1, 51).Select(_ => new PurchaseRequisitionItem()).ToList() };
        var results = vm.Validate(new ValidationContext(vm)).ToList();
        results.Should().Contain(r => r.ErrorMessage!.Contains("maximum of 50"));
    }

    [Fact]
    public void Validate_ShouldFail_WhenItemsAreEmpty()
    {
        var vm = new PurchaseRequisitionVM { Items = [] };
        var results = vm.Validate(new ValidationContext(vm)).ToList();
        results.Should().Contain(r => r.ErrorMessage!.Contains("At least one line item"));
    }
}
