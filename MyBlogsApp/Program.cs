using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyBlogsApp.Data;
using MyBlogsApp.Migrations;
using MyBlogsApp.Services;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDBContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnString")); 
}); 


//Add JWT Token & configure basic settings 
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
    };
});


//Implmenting Redis Cache 
builder.Services.AddStackExchangeRedisCache(options =>
{
    //options.Configuration = "localhost"; 
    //options.Configuration = "host.docker.internal"; 
    options.Configuration = "redis:6379";
    options.InstanceName = "SampleInstance";
});


builder.Services.AddControllers().AddJsonOptions(options => 
{
    // Serialize enums as strings in api responses (e.g. Gender)
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // Ignore default nulls
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    // Ignore possible object cycles
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Implementing Rate Limits 
//https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#setup

// Needed to store rate limit counters and ip rules
builder.Services.AddMemoryCache();

// General configuration from appsettings.json
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

// Inject counter and rules stores
builder.Services.AddInMemoryRateLimiting();

// Add the rate limiter service as Sigleton 
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


//Add CQRS Services 
builder.Services.AddScoped<IPostCommandService, PostCommandService>(); 
builder.Services.AddScoped<IPostQueryService, PostQueryService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// Using IP based rate limiting, the package also provides a custom client ID based limiting as well. 
app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await MigrationHelper.ApplyDatabaseMigrationsAsync(app.Services);

app.Run();
