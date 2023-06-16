using Catalog.API.Extensions;
using CommonLibrary.AzureKeyVaultConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.ConfigureAzureKeyVault();

builder.Services.ConfigureDependedServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureHttpRequestPipeline();

app.Run();
