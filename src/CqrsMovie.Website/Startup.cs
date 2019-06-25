using CqrsMovie.Website.Infrastructure.MassTransit.Events;
using CqrsMovie.Website.Infrastructure.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Muflone.MassTransit.RabbitMQ;
using Muflone.MassTransit.RabbitMQ.Dependecies;

namespace CqrsMovie.Website
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
      services.AddMongoDb(Configuration.GetConnectionString("MongoDB"));
      services.Configure<ServiceBusOptions>(Configuration.GetSection("MassTransit:RabbitMQ"));
      var serviceBusOptions = new ServiceBusOptions();
      Configuration.GetSection("MassTransit:RabbitMQ").Bind(serviceBusOptions);
      services.AddMufloneMassTransitWithRabbitMQ(serviceBusOptions, x =>
      {
        x.AddConsumer<DailyProgrammingCreatedConsumer>();
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      else
        app.UseExceptionHandler("/Home/Error");

      app.UseStaticFiles();
      app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
    }
  }
}