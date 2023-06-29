using CosmeticWeb.Data;
using CosmeticWeb.Models;
using CosmeticWeb.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services
    .AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache();
builder.Services.AddSession();

//builder.Services.AddAuthentication()
//    .AddGoogle(googleOptions =>
//    {
//        googleOptions.ClientId = "";
//        googleOptions.ClientSecret = "";
//    });


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.ConsentCookie.Name = "YourCookieName"; // Replace with your cookie name
    options.ConsentCookie.HttpOnly = true; // Adjust based on your requirements
    options.ConsentCookie.SameSite = SameSiteMode.None; // Adjust based on your requirements
    options.ConsentCookie.SecurePolicy = CookieSecurePolicy.Always; // Adjust based on your requirements
});

var app = builder.Build();

await OrdersSeeds.SeedOrders(app, builder.Configuration);

await UserSeeds.SeedUsersAndRolesAsync(app, builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseStatusCodePagesWithReExecute("/ErrorHandler/{0}");
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseStatusCodePagesWithReExecute("/ErrorHandler/{0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseCookiePolicy();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
