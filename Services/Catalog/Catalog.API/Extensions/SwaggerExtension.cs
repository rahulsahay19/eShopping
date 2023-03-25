using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Catalog.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, string virtualPath,
        IConfiguration config, IApiVersionDescriptionProvider provider)
    {
        var clientId = config.GetValue<string>("AuthN:SwaggerClientId");
        app
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"{virtualPath}/swagger/{description.GroupName}/swagger.json",
                        $"EShopping API {description.GroupName.ToUpperInvariant()}");
                    options.RoutePrefix = string.Empty;
                }

                options.DocumentTitle = "EShopping API Documentation";
                options.OAuthClientId(clientId);
                options.OAuthAppName("EShopping");
                options.OAuthUsePkce();
            });

        return app;
    }
}

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IConfiguration _config;
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IConfiguration config, IApiVersionDescriptionProvider provider)
    {
        _config = config;
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        var disco = GetDiscoveryDocument();

        var apiScope = _config.GetValue<string>("AuthN:ApiName");
        var scopes = apiScope.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList();

        var additionalScopes = _config.GetValue<string>("AuthN:AdditionalScopes");
        scopes.AddRange(additionalScopes.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList());

        var oauthScopeDic = new Dictionary<string, string>();
        foreach (var scope in scopes)
        {
            oauthScopeDic.Add(scope, $"Resource access: {scope}");
        }

        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = $"Catalog.API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                });
        }

        options.EnableAnnotations();

        // options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        // {
        //     Type = SecuritySchemeType.OpenIdConnect,
        //     Flows = new OpenApiOAuthFlows()
        //     {
        //         AuthorizationCode = new OpenApiOAuthFlow
        //         {
        //             AuthorizationUrl = new Uri(disco.AuthorizeEndpoint),
        //             TokenUrl = new Uri(disco.TokenEndpoint),
        //             Scopes = oauthScopeDic
        //         }
        //     }
        // });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
                },
                oauthScopeDic.Keys.ToArray()
            }
        });
    }

    private DiscoveryDocumentResponse GetDiscoveryDocument()
    {
        var client = new HttpClient();
        var authority = _config.GetValue<string>("AuthN:Authority");
        return client.GetDiscoveryDocumentAsync(authority)
            .GetAwaiter()
            .GetResult();
    }
}