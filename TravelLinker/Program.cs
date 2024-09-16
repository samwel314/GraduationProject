using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using TravelLinker.Helpers;
using TravelLinkerDataAccess.Data;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddScoped<IHotelService,HotelService>();
builder.Services.AddScoped<IUserService,  UserService>();
builder.Services.AddScoped<IRoomService,  RoomService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IVehicleScheduleService, VehicleScheduleService>();
builder.Services.AddScoped<IRoomScheduleService, RoomScheduleService>();
//
// Authorization------------------


/// to Authorization Hotel Controller 
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsHotel", builder => builder.RequireClaim
    (StaticData.UserTypeClaim, StaticData.HotelType));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsCompany", builder => builder.RequireClaim
    (StaticData.UserTypeClaim, StaticData.CompanyType));
/// to Authorization Profile Controller  Allow For All Users But not Admin 

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsEnterprise", builder => builder.RequireClaim
    (StaticData.UserTypeClaim , StaticData.HotelType, StaticData.CompanyType));

// Api's
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsCustomer", builder => builder.RequireClaim
    (StaticData.UserTypeClaim , StaticData.CustomerType ));
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsAdmin", builder => builder.RequireClaim
    (StaticData.UserTypeClaim, StaticData.AdminType));
//--------------------------Room Controller ---------Soon -------------------------
// need to IsHotel Policy   and***   Authorization By Resource 
//Chapter 24 last Part 
//----------------
// Work With Identity 

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme , options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
       
        ValidIssuer = builder.Configuration["JwtSettings:validIssuer"],
        ValidAudience = builder.Configuration["JwtSettings:validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:key"]!))
    };
});



builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    //options.SignIn.RequireConfirmedAccount = true;
    options.Lockout.AllowedForNewUsers = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactNativeApp",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
StripeConfiguration.ApiKey
    = builder.Configuration["Stripe:Secretkey"];
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();   
app.MapRazorPages();

app.Run();
