using CqrsMovie.Seats.Infrastructure.MassTransit.Commands;
using CqrsMovie.Seats.Infrastructure.MassTransit.Events;
using CqrsMovie.Seats.Infrastructure.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			services.AddMongoDB(Configuration.GetConnectionString("MongoDB"));
			services.AddMufloneEventStore(Configuration.GetConnectionString("EventStore"));

			services.Configure<ServiceBusOptions>(Configuration.GetSection("MassTransit:RabbitMQ"));
			var serviceBusOptions = new ServiceBusOptions();
			Configuration.GetSection("MassTransit:RabbitMQ").Bind(serviceBusOptions);

			services.AddMufloneMassTransitWithRabbitMQ(serviceBusOptions, x =>
			{
				x.AddConsumer<CreateDailyProgrammingConsumer>();
				x.AddConsumer<DailyProgrammingCreatedConsumer>();
			});

			services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "CQRS Movie Seats API", Version = "v1", Description = "Web Api Services for CQRS-ES workshop" }); });
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseHsts();

			app.UseAuthentication();
			app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CQRS Movie Seats API v1"); });
		}

	}
}