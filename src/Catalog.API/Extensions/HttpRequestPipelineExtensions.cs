using CommonLibrary.HealthChecks;

namespace Catalog.API.Extensions;

public static class HttpRequestPipelineExtensions
{

    public static WebApplication ConfigureHttpRequestPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("AllowAll");
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/", () => "Please use /swagger to see the Catalog.API documentation.");

        app.MapPlayEconomyHealthChecks();

        return app;
    }
}
