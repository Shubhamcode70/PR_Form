using Microsoft.AspNetCore.Mvc;
using PRChecksheetApp.Models;
using PRChecksheetApp.Services.Interfaces;

namespace PRChecksheetApp.Controllers;

public class PurchaseRequisitionController(IPRService service, IWebHostEnvironment environment) : Controller
{
    private readonly IPRService _service = service;
    private readonly IWebHostEnvironment _environment = environment;

    [HttpGet]
    public IActionResult Index() => View(new PurchaseRequisitionVM());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(PurchaseRequisitionVM vm, IFormFile? attachment)
    {
        if (vm.Items.Count > 50)
        {
            ModelState.AddModelError(nameof(vm.Items), "A single PR can have a maximum of 50 items.");
        }

        if (vm.Header.PRType == "CAPEX")
        {
            if (string.IsNullOrWhiteSpace(vm.Header.CRID))
                ModelState.AddModelError("Header.CRID", "CR ID is mandatory for CAPEX.");
            if (string.IsNullOrWhiteSpace(vm.Header.AssetNumber))
                ModelState.AddModelError("Header.AssetNumber", "Asset Number is mandatory for CAPEX.");
        }

        if (!ModelState.IsValid)
            return View("Index", vm);

        if (attachment is not null)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await attachment.CopyToAsync(stream);
            vm.Header.AttachmentPath = $"/uploads/{uniqueFileName}";
        }

        await _service.SavePRAsync(vm);
        TempData["Success"] = "Purchase Requisition submitted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
