using CqrsMovie.Website.Infrastructure.MassTransit.Events;
using CqrsMovie.Website.Infrastructure.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Muflone.MassTransit.RabbitMQ;

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
      services.AddMvc(option => option.EnableEndpointRouting = false);
      services.AddMongoDb(Configuration.GetConnectionString("MongoDB"));
      services.Configure<ServiceBusOptions>(Configuration.GetSection("MassTransit:RabbitMQ"));
      var serviceBusOptions = new ServiceBusOptions();
      Configuration.GetSection("MassTransit:RabbitMQ").Bind(serviceBusOptions);
      services.AddMufloneMassTransitWithRabbitMQ(serviceBusOptions, x =>
      {
        x.AddConsumer<DailyProgrammingCreatedConsumer>();
        x.AddConsumer<SeatsReservedConsumer>();
        x.AddConsumer<SeatsBookedConsumer>();
        //x.AddConsumer<StartBookSeatsSagaConsumer>();
      });
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();
      else
        app.UseExceptionHandler("/Home/Error");

      app.UseStaticFiles();
      app.UseRouting();
      app.UseEndpoints(endpoints =>
      {
          endpoints.MapControllerRoute(
              "default",
              "{controller=Home}/{action=Index}/{id?}");
          endpoints.MapRazorPages();
      });
    }
  }
}