using eCommerceApi.Data;
using eCommerceApi.Middleware;
using eCommerceApi.Repository;
using eCommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddAzureMonitorTraceExporter(options =>
            {
                options.ConnectionString = 
                    builder.Configuration[""];
            });
    })
    .WithMetrics(metrics =>
    {
        metrics 
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddAzureMonitorMetricExporter(options =>
            {
                options.ConnectionString = 
                    builder.Configuration[""];
            });
    });

builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(
        ResourceBuilder.CreateDefault()
    );
    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;
    options.ParseStateValues = true;

    options.AddConsoleExporter();
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

if(app.Environment.IsDevelopment())
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();