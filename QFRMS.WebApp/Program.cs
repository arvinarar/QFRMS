using Microsoft.AspNetCore.Http.Features;
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

var connectionString = builder.Configuration.GetConnectionString("Deployed"); //Change to 'Deployed' if set system to production, 'Development' otherwise

//Add Logger
builder.Services.AddSingleton<IFileLogger, FileLogger>();

builder.Services.AddScoped<Work, Work>();

//Add Scoped Service Dependencies
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IMemoService, MemoService>();
builder.Services.AddScoped<IBatchService, BatchService>();
builder.Services.AddScoped<IStudentService, StudentService>();

//Add Scoped Repository Dependencies
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();
builder.Services.AddScoped<IAboutRepository, AboutRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IMemoRepository, MemoRepository>();
builder.Services.AddScoped<IBatchRepository, BatchRepository>();
builder.Services.AddScoped<IPDFRepository, PDFRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer(connectionString,
        providerOptions => providerOptions.EnableRetryOnFailure())
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

//Set File Size limit to 512MB, for IIS set it on config 536870912(size in bytes)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 512 * 1024 * 1024;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 512 * 1024 * 1024;
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
