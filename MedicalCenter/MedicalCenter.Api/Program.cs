using MedicalCenter.Data;
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

    // Add services to the container.
    string? connectionString = builder.Configuration.GetConnectionString("DbConnection");

    builder.Services.AddDataServices(connectionString, builder.Environment.IsDevelopment());

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

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