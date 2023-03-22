using Catalog.API.Configuration;
using Catalog.Data.Entities;
using CommonLibrary.Identity;
using CommonLibrary.MassTransit;
using CommonLibrary.MongoDB.Extensions;
using CommonLibrary.Settings;

namespace Catalog.API.Extensions;

public static class DependedServicesExtensions
{

    public static IServiceCollection ConfigureDependedServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddSingleton(configuration?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!);

        _ = services.AddSingleton(configuration?.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!);

        _ = services.AddSingleton(configuration?.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>()!);

        var mongoDbCollectionSettings = configuration?.GetSection(nameof(MongoDbCollectionSettings))?.Get<MongoDbCollectionSettings>()!;

        _ = services.AddMongo()
            .AddMongoRepository<CatalogItem>(mongoDbCollectionSettings.Name)
            .AddMassTransitWithRabbitMq()
            .AddJwtBearerAuthentication();

        _ = services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizePolicies.Read, policy =>
            {
                policy.RequireRole("Admin");
                policy.RequireClaim("scope", "catalog.readaccess", "catalog.fullaccess");
            });

            options.AddPolicy(AuthorizePolicies.Write, policy =>
            {
                policy.RequireRole("Admin");
                policy.RequireClaim("scope", "catalog.writeaccess", "catalog.fullaccess");
            });
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


