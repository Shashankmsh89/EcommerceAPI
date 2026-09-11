using EcommerceAPI.Mappings;
using EcommerceAPI.Middleware;
using EcommerceAPI.Repositories;
using EcommerceAPI.Services;
using EcommerceAPI.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMemoryCache();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter =
        PartitionedRateLimiter.Create<HttpContext, string>(
            httpContext =>
            {
                string userKey;

                if (httpContext.User.Identity?.IsAuthenticated == true)
                {
                    userKey =
                        httpContext.User.FindFirst(
                            System.Security.Claims.ClaimTypes.NameIdentifier
                        )?.Value
                        ?? "authenticated-user";
                }
                else
                {
                    userKey =
                        httpContext.Connection.RemoteIpAddress?
                            .ToString()
                        ?? "unknown";
                }

                return RateLimitPartition.GetFixedWindowLimiter(
                    userKey,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder =
                            QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });
});

builder.Services.AddSingleton<IProductCacheService, ProductCacheService>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);

    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;

    options.ApiVersionReader =
    new Asp.Versioning.UrlSegmentApiVersionReader();
});

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ProductProfile>();
    cfg.AddProfile<CategoryProfile>();
    cfg.AddProfile<ProductPriceProfile>();
});

builder.Services.AddValidatorsFromAssemblyContaining<ProductRequestValidator>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductPriceRepository,ProductPriceRepository>();
builder.Services.AddScoped<IProductAvailabilityRepository, ProductAvailabilityRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<ICheckoutRepository, CheckoutRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IRefreshTokenRepository,RefreshTokenRepository>();

builder.Services.AddOpenApi();

var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey!)),

                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors("FrontendPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();