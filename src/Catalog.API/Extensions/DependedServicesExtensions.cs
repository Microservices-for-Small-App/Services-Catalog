﻿using Catalog.ApplicationCore.Settings;
using Microsoft.Extensions.Configuration;
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

        var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

        var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

        _ = services.AddSingleton(serviceProvider =>
        {
            return new MongoClient(mongoDbSettings?.ConnectionString)
                .GetDatabase(serviceSettings?.ServiceName);
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
//builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

