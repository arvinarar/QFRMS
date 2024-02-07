using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QFRMS.Data;
using QFRMS.Data.Interfaces;
using QFRMS.Data.Models;
using QFRMS.Data.Repositories;
using QFRMS.Services.Interfaces;
using QFRMS.Services.Services;
using QFRMS.Services.Utils;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("default");

//Add Logger
builder.Services.AddSingleton<IFileLogger, FileLogger>();

//Add Scoped Service Dependencies
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

//Add Scoped Repository Dependencies
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer(connectionString)
    );

builder.Services.AddIdentity<UserAccount, IdentityRole>(
    options =>
        {
            options.User.RequireUniqueEmail = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 1;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
        })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(5);
});

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
