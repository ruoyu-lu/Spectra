using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Spectra.ApiService.Configuration;
using Spectra.Application.Interfaces;
using Spectra.Application.Services;
using Spectra.Infrastructure.Data;
using Spectra.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Add Entity Framework with PostgreSQL via Aspire
builder.AddNpgsqlDbContext<ApplicationDbContext>("spectra-db", configureDbContextOptions: options =>
{
    options.UseNpgsql(npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly("Spectra.Infrastructure");
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
    });

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Add authentication services
builder.Services.AddAuthenticationServices(builder.Configuration);

// Add application services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageService, ImageService>();

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

// Add controllers
builder.Services.AddControllers();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Spectra API",
        Version = "v1",
        Description = "Cloud-native image social platform API"
    });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add health checks including PostgreSQL
builder.Services.AddHealthChecks()
    .AddNpgSql(name: "postgresql");

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spectra API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Enable serving static files from wwwroot
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultEndpoints(); // This already includes health checks

// In development, ensure database is created with retry logic
if (!app.Environment.IsProduction())
{
    await EnsureDatabaseCreatedAsync(app.Services);
}

async Task EnsureDatabaseCreatedAsync(IServiceProvider services)
{
    const int maxRetries = 10;
    const int delayMs = 2000;

    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
            break;
        }
        catch (Exception ex) when (i < maxRetries - 1)
        {
            app.Logger.LogWarning("Failed to connect to database (attempt {Attempt}/{MaxRetries}): {Error}",
                i + 1, maxRetries, ex.Message);
            await Task.Delay(delayMs);
        }
    }
}

app.Run();
