using jh_payment_database.DatabaseContext;
using jh_payment_database.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// build absolute path for DB (ensure directory exists)
var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
Directory.CreateDirectory(dataDir);
var dbPath = Path.Combine(dataDir, "jh_poc_payments.db");
var connStr = $"Data Source={dbPath}";

// register DbContext
builder.Services.AddDbContext<JHDataAccessContext>(options =>
    options.UseSqlite(connStr));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<OperationService>();

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // Default: v1.0
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true; // Shows supported versions in headers

    // Accept version from URL, header, or query string
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),   // ?api-version=1.0
        new HeaderApiVersionReader("X-Version"),          // Header: X-Version: 1.0
        new MediaTypeApiVersionReader("ver"));            // Header: Content-Type: application/json;ver=1.0
});

var app = builder.Build();

// apply migrations at startup (careful in multi-instance setups)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<JHDataAccessContext>();

    var flag = builder.Configuration.GetSection("ReCreateDatabase").Value;
    if (flag != null && flag == "True")
    {
        db.Database.EnsureDeleted();
    }

    if (!db.Database.CanConnect())
    {
        db.Database.EnsureCreated();        
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
