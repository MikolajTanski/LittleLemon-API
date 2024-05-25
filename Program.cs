using Azure.Identity;
using LittleLemon_API.Data;
using LittleLemon_API.Middleware;
using LittleLemon_API.Repository.MealRepository;
using LittleLemon_API.Repository.OrderRepository;
using LittleLemon_API.Services.EmailServices;
using LittleLemon_API.Services.ImageService;
using LittleLemon_API.Services.MealService;
using LittleLemon_API.Services.OrderService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<ErrorHandlingMiddleware>();
builder.Services.AddSingleton<SwaggerBasicAuthMiddleware>();

builder.Services.AddScoped<IMealRepository, MealRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMealService, MealService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IImageService, ImageService>();
// Ensure that the KeyVaultName setting is properly retrieved from the configuration
var keyVaultName = builder.Configuration["KeyVaultName"];
if (string.IsNullOrEmpty(keyVaultName))
{
    throw new InvalidOperationException("KeyVaultName is not configured properly in appsettings.json.");
}

Console.WriteLine($"Using Key Vault: {keyVaultName}");

var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

// Retrieve the connection string from Azure Key Vault
var connectionString = builder.Configuration["DefaultConnection"];
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("DefaultConnection is not configured properly in Azure Key Vault.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Little Lemon API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }});
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var configuration = services.GetRequiredService<IConfiguration>();

    try
    {
        // Check if there is any table in the database
        var databaseExists = context.Database.GetService<IRelationalDatabaseCreator>().Exists();

        if (!databaseExists)
        {
            context.Database.Migrate();
            SeedData.Initialize(services, configuration);
        }
        else
        {
            Console.WriteLine("Database already contains data. Skipping migration and seeding.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while checking the database: {ex.Message}");
    }
}

app.MapControllers();
app.UseErrorHandling();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Little Lemon API V1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();
app.Run();