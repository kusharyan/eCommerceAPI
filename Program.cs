using eCommerceApi.Data;
using eCommerceApi.Middleware;
using eCommerceApi.Repository;
using eCommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using System.Threading;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter()
    );

builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(
        ResourceBuilder.CreateDefault()
    );
    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;
    options.ParseStateValues = true;

    options.AddConsoleExporter();  
    options.AddOtlpExporter();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
    ));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductsRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    const int maxAttempts = 12;
    const int delayMs = 5000;
    for (int attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            db.Database.Migrate();
            logger.LogInformation("Database migrated successfully on attempt {Attempt}", attempt);
            break;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Database migrate attempt {Attempt} failed", attempt);
            if (attempt == maxAttempts)
            {
                logger.LogError(ex, "All database migration attempts failed");
                throw;
            }
            Thread.Sleep(delayMs);
        }
    }
}

if(app.Environment.IsDevelopment())
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();