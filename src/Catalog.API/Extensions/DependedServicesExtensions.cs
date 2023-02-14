using Catalog.ApplicationCore.Interfaces;
using Catalog.ApplicationCore.Settings;
using Catalog.Data.Entities;
using Catalog.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Catalog.API.Extensions;

public static class DependedServicesExtensions
{

    public static IServiceCollection ConfigureDependedServices(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        _ = services.AddSingleton(serviceProvider =>
        {
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

            return new MongoClient(mongoDbSettings?.ConnectionString)
                .GetDatabase(serviceSettings?.ServiceName);
        });

        _ = services.Configure<MongoDbCollectionSettings>(configuration.GetSection(nameof(MongoDbCollectionSettings)));

        _ = services.AddSingleton<IRepository<Item>>(serviceProvider =>
        {
            var mongoDbCollectionSettings = configuration.GetSection(nameof(MongoDbCollectionSettings)).Get<MongoDbCollectionSettings>();

            return new MongoRepository<Item>(serviceProvider?.GetService<IMongoDatabase>()!, mongoDbCollectionSettings?.Name!);
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
            options.AddPolicy("AllowAll", policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
        });

        return services;
    }

}

//builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection("ServiceSettings"));


