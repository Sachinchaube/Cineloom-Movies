using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieBookingPro.DAL;
using MovieBookingPro.Models;
using MovieBookingPro.Repository;
using MovieBookingPro.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC + Areas
builder.Services.AddControllersWithViews();

// EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);
// HttpClient for AI recommendation service
builder.Services.AddHttpClient<IRecommendationService, RecommendationService>();

// Repositories
builder.Services.AddScoped<IMovieRepo, MovieRepo>();
builder.Services.AddScoped<ITheatreRepo, TheatreRepo>();
builder.Services.AddScoped<IScreenRepo, ScreenRepo>();
builder.Services.AddScoped<IShowScheduleRepo, ShowScheduleRepo>();
builder.Services.AddScoped<IBookingRepo, BookingRepo>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed database with roles, users, movies, theatres, screens, and show schedules
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    await DbInitializer.SeedAsync(context, userManager, roleManager);
}

app.Run();
