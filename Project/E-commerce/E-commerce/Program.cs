using DAL;
using E_commerce.AttributeOrFilter;
using E_commerce.Extension;
using E_commerce.Middleware;
using E_commerce.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using System.Text;
using Utils.Enums;
using Utils.Tools;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Register log event to DBL
DBL.Tools.LogHelper.OnLogEvent += LogHelper.LogMessage;

// Register AuthToken as a singleton
builder.Services.AddSingleton<AuthToken>();
builder.Services.AddSingleton<EncryptionHelper>();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(CustomAuthorizeFilter)); // Add the custom authorization filter globally
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load the key from appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidIssuer = issuer,
        ValidAudience = audience
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["authToken"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomMerchantAccess", policy =>
        policy.RequireAssertion(context =>
        {
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == Enum_UserRole.Admin.ToString() || userRole == Enum_UserRole.Merchant.ToString())
            {
                return true; // Admins & Merchant have access
            }

            return false;
        }));
});

var reactSettings = builder.Configuration.GetSection("ReactSettings");
var reactBaseUrl = reactSettings["BaseUrl"];


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder
            .WithOrigins(reactBaseUrl)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); // Allow credentials like cookies
});

// Add services to the container.
var sqlConnString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSqlServer<MyDbContext>(sqlConnString);

// Auto Add All Services
builder.Services.AddAllService();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS policy before authentication
app.UseCors("AllowLocalhost");

// Register custom JWT middleware before authentication and authorization
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Auto Create Database or Update Database table
// Not sure can use in production or not
//app.CreatOrUpdateDatabase();

app.Run();
