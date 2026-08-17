using Microsoft.EntityFrameworkCore;

using PaintERP.Data;

using PaintERP.Services;
using PaintERP.Extensions;
using PaintERP.Middleware;


var builder = WebApplication.CreateBuilder(args);



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")

                      ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");



builder.Services.AddDbContext<PaintErpDbContext>(options =>

    options.UseSqlServer(connectionString, sql =>

        sql.EnableRetryOnFailure(maxRetryCount: 3,

            maxRetryDelay: TimeSpan.FromSeconds(5),

            errorNumbersToAdd: null)));

builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddApplicationServices();


// Add Authentication and Authorization

builder.Services.AddAuthentication(options =>

{

    options.DefaultScheme = "Cookies";

})

.AddCookie("Cookies");



// Add services to the container.

builder.Services.AddControllersWithViews();



var app = builder.Build();



// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())

{

    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

    app.UseHsts();

}



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Seed default unit conversions
using (var scope = app.Services.CreateScope())
{
    var unitConversionService = scope.ServiceProvider.GetRequiredService<UnitConversionService>();
    await unitConversionService.SeedDefaultConversionsAsync();
}

app.UseAuthentication();

app.UseAuthorization();



app.MapStaticAssets();



app.MapControllerRoute(

    name: "default",

    pattern: "{controller=Home}/{action=Index}/{id?}")

    .WithStaticAssets();





app.Run();

