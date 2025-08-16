using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using RealEstate.API.Extensions;
using RealEstate.Application;
using RealEstate.Application.Handlers.Owner;
using RealEstate.Application.LogServices.CreateError;
using RealEstate.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.CustomAddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Dependency Injection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateOwnertHandler).Assembly);

});


// Application & Infrastructure
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
//    typeof(RealEstate.Application.Commands.CreateOwnerCommand).Assembly));


builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var app = builder.Build();
app.UseCors("AllowAll");
app.MapHealthChecks("/health");

// Manejo global de errores
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error != null)
        {
            var mediator = context.RequestServices.GetRequiredService<IMediator>();

            var result = await mediator.Send(new CreateErrorCommand()
            {
                SeverityID = 5,
                Description = exceptionHandlerPathFeature.Error.ToString(),
                Component = "Transaction.API",
                Date = DateTime.UtcNow.AddHours(-5)
            });

            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Ocurrió un error inesperado. Por favor, intente nuevamente.",
            });
        }
    });
});

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.CustomMapOpenApi();
}

//app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
