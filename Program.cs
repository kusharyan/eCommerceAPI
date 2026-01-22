using eCommerceApi.Data;
using eCommerceApi.Middleware;
using eCommerceApi.Repository;
using eCommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
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
    Console.WriteLine(">>> MIGRATION STARTING <<<");

    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate();
        Console.WriteLine(">>> MIGRATION SUCCESS <<<");
    }
    catch (Exception ex)
    {
        Console.WriteLine(">>> MIGRATION FAILED <<<");
        Console.WriteLine(ex.ToString());
    }
}

if(app.Environment.IsDevelopment())
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();