using Catalog.Data.Entities;
using CommonLibrary.MongoDB.Extensions;
using CommonLibrary.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Play.Common.MassTransit;

namespace Catalog.API.Extensions;

public static class DependedServicesExtensions
{

    public static IServiceCollection ConfigureDependedServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddSingleton(serviceProvider =>
        {
            return serviceProvider.GetService<IConfiguration>()?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;
        });

        _ = services.AddSingleton(serviceProvider =>
        {
            return serviceProvider.GetService<IConfiguration>()?.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!;
        });

        _ = services.AddSingleton(serviceProvider =>
        {
            return serviceProvider.GetService<IConfiguration>()?.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>()!;
        });

        var mongoDbCollectionSettings = configuration?.GetSection(nameof(MongoDbCollectionSettings))?.Get<MongoDbCollectionSettings>()!;

        _ = services.AddMongo()
            .AddMongoRepository<CatalogItem>(mongoDbCollectionSettings.Name)
        .AddMassTransitWithRabbitMq();

        _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5003";
                    options.Audience = configuration?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()?.ServiceName;
                });

        _ = services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        _ = services.AddEndpointsApiExplorer();
        _ = services.AddSwaggerGen();

        _ = services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => policy.WithOrigins(configuration?["AllowedOrigin"]!)
                                                            .AllowAnyHeader().AllowAnyMethod());
        });

        return services;
    }

}

//builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection("ServiceSettings"));
//_ = services.AddSingleton(configuration?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!);
//_ = services.AddSingleton(configuration?.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!);
//_ = services.AddSingleton(configuration?.GetSection(nameof(MongoDbCollectionSettings)).Get<MongoDbCollectionSettings>()!);


