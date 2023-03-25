using EventBus.Messages.Common;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;

namespace Ordering.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();
        services.AddApplicationServices();
        services.AddInfraServices(Configuration);
        services.AddAutoMapper(typeof(Startup));
        services.AddScoped<BasketOrderingConsumer>();
        services.AddScoped<BasketOrderingConsumerV2>();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "Ordering.API", Version = "v1"});
        });
        services.AddHealthChecks().Services.AddDbContext<OrderContext>();
        services.AddMassTransit(config =>
        {
            //Mark this as consumer
            config.AddConsumer<BasketOrderingConsumer>();
            config.AddConsumer<BasketOrderingConsumerV2>();
            config.UsingRabbitMq((ctx, cfg)=>
            {
                cfg.Host(Configuration["EventBusSettings:HostAddress"]);
                //provide the queue name with consumer settings
                cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
                {
                    c.ConfigureConsumer<BasketOrderingConsumer>(ctx);  
                });
                //V2 endpoint will pick items from here 
                cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueueV2, c =>
                {
                    c.ConfigureConsumer<BasketOrderingConsumerV2>(ctx);  
                });
            });
        });
        services.AddMassTransitHostedService();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
        }

        app.UseRouting();
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