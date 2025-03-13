//วิธีที่ดีที่สุดคือ แยกโค้ดออกเป็นชั้น(Layered Architecture) โดยสามารถแบ่งออกเป็น 3 ส่วนหลัก ๆ:

//1 Data Access Layer (DAL) – connent DB
//2 Service Layer (Business Logic) – Process data
//3 Controller (API Layer) – Manage requests HTTP

/// MyProject
// ├── / Controllers
// │   ├── UsersController.cs
// ├── / Services
// │   ├── IUserService.cs
// │   ├── UserService.cs
// ├── / Repositories
// │   ├── IUserRepository.cs
// │   ├── UserRepository.cs
// ├── / Models
// │   ├── User.cs
// ├── appsettings.json
// ├── Program.cs
// ├── Startup.cs





using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FMoneAPI.Data;
using FMoneAPI.Repositories.UserRepository;
using FMoneAPI.Services.UserService;
using FMoneAPI;
using FMoneAPI.Repositories.BannerRepository;
using FMoneAPI.Services.BannerService;
using FMoneAPI.Repositories.NewsCategoryRepository;
using FMoneAPI.Services.NewsCategoryService;
using FMoneAPI.Services.NewsService;
using FMoneAPI.Repositories.NewsRepository;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"),
    new MySqlServerVersion(new Version(8, 0, 26))));

// Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FMoneAPI",
        Version = "v1"
    });
});

// add IConfiguration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// register Repository and Service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBannerRepository, BannerRepository>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<INewsCategoryRepository, NewsCategoryRepository>();
builder.Services.AddScoped<INewsCategoryService, NewsCategoryService>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();

// add Authorization Middleware
builder.Services.AddAuthorization();

// add Controller
builder.Services.AddControllers();
// Listen at 0.0.0.0 for other machines.
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000);  // setting port (port: 5000)
});

builder.Services.AddAutoMapper(typeof(UserProfile));  // add AutoMapper

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                "http://localhost",
                "http://localhost:3000",
                "http://localhost:5000",
                "http://119.59.118.117",
                "http://119.59.118.117:18988",
                "http://119.59.118.117:5000"
                ) // enable URL of client
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins); // add CORS

// add Middleware
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
