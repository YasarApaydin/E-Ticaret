using E_Ticaret.Application.DependencyInjection;
using E_Ticaret.Infrastructure.BackgroundServices;
using E_Ticaret.Persistence.DependencyInjection;
using E_Ticaret.Presentation.Controllers;
using E_Ticaret.WebApý;

using E_Ticaret.WebApý.Modules;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});


builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(10);
});



builder.Services.AddApplication();
builder.Services.AddPersistance(builder.Configuration);

builder.Services.AddHostedService<ExpiredVerificationCleaner>();





builder.Services.AddControllers().AddOData(opt=> opt.Select().Filter().Count().Expand().OrderBy().SetMaxTop(null).AddRouteComponents("odata",AppODataController.GetEdmModel())

);



builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

   
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: "Global",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 500,
                Window = TimeSpan.FromSeconds(10),
                QueueLimit = 0
            });
    });

  
    options.AddPolicy("Login", httpContext =>
    {
        var key = httpContext.User?.Identity?.IsAuthenticated == true
            ? httpContext.User.Identity.Name!
            : httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetTokenBucketLimiter(
            partitionKey: key,
            factory: _ => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 5,
                TokensPerPeriod = 5,
                ReplenishmentPeriod = TimeSpan.FromSeconds(30),
                QueueLimit = 0,
                AutoReplenishment = true
            });
    });

 
    options.AddPolicy("Register", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            ip,
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });

    
    options.AddPolicy("Search", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            ip,
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 50,
                Window = TimeSpan.FromSeconds(5),
                QueueLimit = 0
            });
    });


    options.AddPolicy("Checkout", httpContext =>
    {
        var key = httpContext.User?.Identity?.IsAuthenticated == true
            ? httpContext.User.Identity.Name!
            : httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetSlidingWindowLimiter(
            key,
            _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 3,
                QueueLimit = 0
            });
    });

 
});


builder.Services.AddOpenApi();




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

var app = builder.Build();



app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();


app.UseCors("AllowAll");
app.RegisterRoutes();
app.UseAuthentication();
app.UseAuthorization();


app.UseResponseCompression();
app.UseExceptionHandler();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRateLimiter();


app.MapControllers().RequireAuthorization();

app.Run();
 