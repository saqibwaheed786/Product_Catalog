using Microsoft.EntityFrameworkCore;
using ProductCatalog.Repositories;
using ProductCatalog.Data;
using ProductCatalog.Manager;
using ProductCatalog.Models;
using RMS.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProductCatalogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectCatalogDbString") ??
                               throw new InvalidOperationException("Connection string 'ProjectCatalogDbString' not found.")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ProductCatalogManager>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAuthentication("Cookies")
       .AddCookie("Cookies", options =>
       {
           options.LoginPath = "/User/Login";
           // Other cookie options if needed
       });

builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(15);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
