﻿using Catalog.ApplicationCore.Settings;
using Catalog.Data.Entities;
using Catalog.Repositories.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API.Extensions;

public static class DependedServicesExtensions
{

    public static IServiceCollection ConfigureDependedServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddSingleton(configuration?.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!);

        _ = services.AddSingleton(configuration?.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!);

        _ = services.AddSingleton(configuration?.GetSection(nameof(MongoDbCollectionSettings)).Get<MongoDbCollectionSettings>()!);

        _ = services.AddMongo().AddMongoRepository<Item>();

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


