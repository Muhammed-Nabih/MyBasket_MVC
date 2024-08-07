using Microsoft.EntityFrameworkCore;
using MyBasket.Domain;
using MyBasket.Domain.Repository;
using MyBasket.Infrastructure.Implementation;
using Microsoft.AspNetCore.Identity;
using MyBasket.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using MyBasket.Infrastructure.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("No connection string was found");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.Configure<StripeData>(builder.Configuration.GetSection("stripe"));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(
	options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4)
	).AddDefaultTokenProviders().AddDefaultUI()
	.AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddSingleton<IEmailSender,EmailSender>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

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

StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();
SeedDb();
app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Customer",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();


void SeedDb()
{
    using(var scope = app.Services.CreateScope())
    {
        var dbInitalizer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitalizer.Initialize();
    }
}