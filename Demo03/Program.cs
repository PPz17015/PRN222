using Demo03.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Move ConfigureServices to a proper location and remove the invalid 'public' modifier
void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<MyStockContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Server=localhost;database=MyStock;uid=sa;pwd=123;TrustServerCertificate=True;")));
    services.AddScoped(typeof(MyStockContext));
    services.AddControllersWithViews();

}

// Call ConfigureServices explicitly to fix CS8321
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
