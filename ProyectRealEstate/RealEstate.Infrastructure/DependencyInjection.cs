using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nuget.LogService.Services;
using RealEstate.Application.Handlers.Owner;
using RealEstate.Application.Mappings;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Repositories;
using RealEstate.Infrastructure.Mongo;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Infrastructure.UnitOfWork;

namespace RealEstate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar MongoDB desde appsettings.json
            services.Configure<MongoDbSettings>(configuration.GetSection("ConnectionStrings"));

            // Registrar MongoClient como singleton (thread-safe)
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<OwnertProfile>();
            });

            // Registrar IMongoDatabase como scoped
            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return client.GetDatabase(settings.DatabaseName);
            });

            // Registrar UnitOfWork y repositorios
            services.AddScoped<IMongoUnitWork, MongoUnitWork>();
            services.AddScoped<IOwnerRepositories, OwnerRepositories>();

            // Configurar servicio de logs
            var logAPIBaseURL = configuration.GetRequiredSection("ApiCentralLog:UrlBase").Value;
            var logAPIToken = configuration.GetRequiredSection("ApiCentralLog:Token").Value;

            services.AddScoped<ILogServices>(c =>
            {
                var logger = c.GetRequiredService<ILogger<LogServices>>();
                return new LogServices(logAPIBaseURL, logAPIToken, logger);
            });

            return services;
        }
    }
}

