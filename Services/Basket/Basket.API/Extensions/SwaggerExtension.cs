using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Basket.API.Extensions;

public static class SwaggerExtension
{
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, string nginxPath, 
         IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger()
        .UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"{nginxPath}/swagger/{description.GroupName}/swagger.json",
                    $"Basket.API v1 {description.GroupName.ToUpperInvariant()}");
                options.RoutePrefix = string.Empty;
            }

            options.DocumentTitle = "Basket.API v1 Documentation";
        });
        return app;
    }
}
