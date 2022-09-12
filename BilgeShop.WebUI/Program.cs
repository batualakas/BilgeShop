
using BilgeShop.Business.Managers;
using BilgeShop.Business.Services;
using BilgeShop.Data.Contexts;
using BilgeShop.Data.Repositories;
using BilgeShop.WebUI.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Bölgesel ayarýn tüm uygulama için ayarlanmasý, eðer parametresiz CultureInfo constuctor'ý kullanýlýrsa tr-TR üzerinden ayarlanýr.
// Önce cultureUtil objesi new'lenir, daha sonra AddCulture methoduyla builder servisleri konfigüre edilir, son olarak da aþaðýda olduðu gibi app.UseRequestLocalization methodu içinde UseCulture methodu çaðrýlýr.
CultureUtil cultureUtil = new CultureUtil("en-US"); // Ýngilizce için parametre en-US gönderilmelidir.

builder.Services.Configure(cultureUtil.AddCulture());

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BilgeShopContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();

builder.Services.AddDataProtection();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/");
});



var app = builder.Build();

app.UseRequestLocalization(cultureUtil.UseCulture());

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{Controller=Dashboard}/{Action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: ("{Controller=Home}/{Action=Index}/{id?}")
    );



app.Run();
