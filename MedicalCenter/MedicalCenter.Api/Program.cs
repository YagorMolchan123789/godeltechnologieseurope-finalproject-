using System.Reflection;
using Asp.Versioning;
using MedicalCenter.Api.Extensions;
using MedicalCenter.Business;
using MedicalCenter.Data;
using MedicalCenter.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
           .WriteTo.Console()
           .CreateBootstrapLogger();

Log.Information("Starting web api");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //Add logging
    builder.Host.UseSerilog((context, services, configuration) => configuration
               .ReadFrom.Configuration(context.Configuration));

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("doctor", policy => policy.RequireRole("doctor"));
        options.AddPolicy("admin", policy => policy.RequireRole("admin"));
    });

    builder.Services.AddIdentityApiEndpoints<AppUser>(options =>
    {
        options.Password.RequiredLength = 8;
    })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    // Add services to the container.
    string? connectionString = builder.Configuration.GetConnectionString("DbConnection");

    builder.Services.AddApiServices();
    builder.Services.AddDataServices(connectionString, builder.Environment.IsDevelopment());
    builder.Services.AddBusinessServices();

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

    builder.Services.AddHealthChecks();

    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MedicalCenterAPI", Version = "v1" });

        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    var apiVersioningBuilder = builder.Services.AddApiVersioning(o =>
    {
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.ReportApiVersions = true;
        o.ApiVersionReader = ApiVersionReader.Combine(
            new QueryStringApiVersionReader("api-version"),
            new HeaderApiVersionReader("X-Version"),
            new MediaTypeApiVersionReader("ver"));
    });

    apiVersioningBuilder.AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("CorsPolicy", policyBuilder =>
        {
            policyBuilder.AllowAnyHeader()
                         .AllowAnyMethod()
                         .WithOrigins("http://localhost:8080", "http://localhost:88")
                         .AllowCredentials();
        });
    });

    var app = builder.Build();

    app.UseCors("CorsPolicy");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapHealthChecks("/health");

    app.MapControllers();

    await IdentitySeedData.CreateAdminAccountAsync(app.Services);
    await DoctorSeedData.CreateDoctorsAccountAsync(app.Services);

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
