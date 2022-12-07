using Microsoft.EntityFrameworkCore;
using AdminApplication.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var DatabaseConnectionString = builder.Configuration.GetConnectionString("ProjectCDBConnection");
builder.Services.AddDbContext<ApplicationDbContext>(
    options => /*options.UseSqlServer(DatabaseConnectionString));*/
options.UseMySql(DatabaseConnectionString, ServerVersion.AutoDetect(DatabaseConnectionString)));
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=LoginPage}/{id?}");

app.Run();
