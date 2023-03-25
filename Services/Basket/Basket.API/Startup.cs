using System.Reflection;
using Basket.API.Swagger;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging.Correlation;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basket.API;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            //Enable when required
            // options.ApiVersionReader = ApiVersionReader.Combine(
            //         new HeaderApiVersionReader("X-Version"),
            //         new QueryStringApiVersionReader("api-version", "ver"),
            //         new MediaTypeApiVersionReader("ver")
            //     );
        });
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        services.AddApiVersioning();
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                //TODO read the same from settings for prod deployment
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });
        });
        //Redis Settings
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
        });
        services.AddMediatR(typeof(CreateShoppingCartCommandHandler).GetTypeInfo().Assembly);
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
        services.AddAutoMapper(typeof(Startup));
        services.AddScoped<DiscountGrpcService>();
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
            (o => o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();
        });
        services.AddHealthChecks()
            .AddRedis(Configuration["CacheSettings:ConnectionString"], "Redis Health", HealthStatus.Degraded);
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ct, cfg)=>
            {
                cfg.Host(Configuration["EventBusSettings:HostAddress"]);
            });
        });
        services.AddMassTransitHostedService();
        //Identity Server changes
        // var userPolicy = new AuthorizationPolicyBuilder()
        //     .RequireAuthenticatedUser()
        //     .Build();
        //
        // services.AddControllers(config =>
        // {
        //     config.Filters.Add(new AuthorizeFilter(userPolicy));
        // });
        //
        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(options =>
        //     {
        //         options.Authority = "https://localhost:9009";
        //         options.Audience = "Basket";
        //     });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();  
            app.UseSwagger();
            app.UseSwaggerUI(options => {

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
        

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}