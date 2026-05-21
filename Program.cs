using Microsoft.EntityFrameworkCore;
using PRChecksheetApp.Data;
using PRChecksheetApp.Repositories;
using PRChecksheetApp.Repositories.Interfaces;
using PRChecksheetApp.Services;
using PRChecksheetApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    ));

builder.Services.AddScoped<IPRRepository, PRRepository>();
builder.Services.AddScoped<IPRService, PRService>();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PurchaseRequisition}/{action=Index}/{id?}");

app.Run();
