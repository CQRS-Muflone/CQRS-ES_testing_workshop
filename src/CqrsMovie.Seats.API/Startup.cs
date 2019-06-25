using System.IO;
using CqrsMovie.Seats.Infrastructure.MassTransit.Commands;
using CqrsMovie.Seats.Infrastructure.MassTransit.Events;
using CqrsMovie.Seats.Infrastructure.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Muflone.Eventstore;
using Muflone.MassTransit.RabbitMQ;
using Swashbuckle.AspNetCore.Swagger;

namespace CqrsMovie.Seats.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
      services.AddMongoDB(Configuration.GetConnectionString("MongoDB"));
      services.AddMufloneEventStore(Configuration.GetConnectionString("EventStore"));

      services.Configure<ServiceBusOptions>(Configuration.GetSection("MassTransit:RabbitMQ"));
      var serviceBusOptions = new ServiceBusOptions();
      Configuration.GetSection("MassTransit:RabbitMQ").Bind(serviceBusOptions);

      services.AddMufloneMassTransitWithRabbitMQ(serviceBusOptions, x =>
      {
        x.AddConsumer<CreateDailyProgrammingConsumer>();
        x.AddConsumer<DailyProgrammingCreatedConsumer>();

        x.AddConsumer<BookSeatsConsumer>();
        x.AddConsumer<SeatsBookedConsumer>();

        x.AddConsumer<ReserveSeatsConsumer>();
        x.AddConsumer<SeatsReservedConsumer>();
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "IAD 2018", Version = "v1", Description = "Web Api Services for CQRS-ES workshop", TermsOfService = "" });

        var pathDoc = "CqrsMovie.Seats.API.xml";

        var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, pathDoc);
        if (File.Exists(filePath))
          c.IncludeXmlComments(filePath);

        c.DescribeAllEnumsAsStrings();
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();
      app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
      app.UseStaticFiles();
      app.UseSwagger(c => { c.RouteTemplate = "documentation/{documentName}/documentation.json"; });
      app.UseSwaggerUI(c => { c.SwaggerEndpoint("/documentation/v1/documentation.json", "IAD Cqrs Movie seats API v1"); });
    }
  }
}