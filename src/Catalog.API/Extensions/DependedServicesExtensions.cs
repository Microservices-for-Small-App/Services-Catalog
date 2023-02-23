using Catalog.Data.Entities;
using CommonLibrary.MongoDB.Extensions;
using CommonLibrary.Settings;
using MassTransit;
using MassTransit.Definition;
using Play.Common.MassTransit;

namespace Catalog.API.Extensions;

public static class DependedServicesExtensions
{

    public static IServiceCollection ConfigureDependedServices(this IServiceCollection services)
    {
        RabbitMQSettings rabbitMQSettings = new();
        ServiceSettings serviceSettings = new();

        _ = services.AddSingleton(serviceProvider =>
        {
            return serviceSettings = serviceProvider.GetService<IConfiguration>()
                    ?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;
        });

        _ = services.AddSingleton(serviceProvider =>
        {
            return serviceProvider.GetService<IConfiguration>()
                    ?.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!;
        });

        _ = services.AddSingleton(serviceProvider =>
        {
            return rabbitMQSettings = serviceProvider.GetService<IConfiguration>()
                    ?.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>()!;
        });

        _ = services.AddSingleton(serviceProvider =>
        {
            return serviceProvider.GetService<IConfiguration>()
                    ?.GetSection(nameof(MongoDbCollectionSettings)).Get<MongoDbCollectionSettings>()!;
        });

        _ = services.AddMongo()
            .AddMongoRepository<CatalogItem>()
            .AddMassTransitWithRabbitMq();

        //_ = services.AddMassTransit(x =>
        //{
        //    x.UsingRabbitMq((context, configurator) =>
        //    {
        //        configurator.Host(rabbitMQSettings.Host);

        //        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
        //    });
        //});

        //_ = services.AddMassTransitHostedService();

        _ = services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        _ = services.AddEndpointsApiExplorer();
        _ = services.AddSwaggerGen();

        _ = services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
        });

        return services;
    }

}

//builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection("ServiceSettings"));
//_ = services.AddSingleton(configuration?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!);
//_ = services.AddSingleton(configuration?.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!);
//_ = services.AddSingleton(configuration?.GetSection(nameof(MongoDbCollectionSettings)).Get<MongoDbCollectionSettings>()!);


