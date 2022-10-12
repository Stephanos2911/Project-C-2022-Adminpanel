using Microsoft.EntityFrameworkCore;
using AdminApplication.Models;
using Project_C.Models.StoreModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var employeeConnectionString = builder.Configuration.GetConnectionString("ProductDBConnection");
builder.Services.AddDbContextPool<AppDbContext>(
    options => options.UseSqlServer(employeeConnectionString));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductRepository, SQLProductRepository>();
builder.Services.AddScoped<IStoreRepository, SQLStoreRepository>();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
