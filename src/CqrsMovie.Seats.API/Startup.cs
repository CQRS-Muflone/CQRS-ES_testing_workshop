using CqrsMovie.Seats.Infrastructure.MassTransit.Commands;
using CqrsMovie.Seats.Infrastructure.MassTransit.Events;
using CqrsMovie.Seats.Infrastructure.MassTransit.Sagas;
using CqrsMovie.Seats.Infrastructure.MongoDb;
using CqrsMovie.Seats.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Muflone.Eventstore;
using Muflone.MassTransit.RabbitMQ;
using Muflone.Saga.Persistence;

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
            services.AddMvc(option => option.EnableEndpointRouting = false);
			services.AddMongoDB(Configuration.GetConnectionString("MongoDB"));
			services.AddMufloneEventStore(Configuration.GetConnectionString("EventStore"));

            services.AddScoped<ISagaRepository, InMemorySagaRepository>();
            services.AddScoped<ISerializer, Serializer>();

			services.Configure<ServiceBusOptions>(Configuration.GetSection("MassTransit:RabbitMQ"));
			var serviceBusOptions = new ServiceBusOptions();
			Configuration.GetSection("MassTransit:RabbitMQ").Bind(serviceBusOptions);

			services.AddMufloneMassTransitWithRabbitMQ(serviceBusOptions, x =>
			{
				x.AddConsumer<CreateDailyProgrammingConsumer>();
				x.AddConsumer<DailyProgrammingCreatedConsumer>();

				x.AddConsumer<BookSeatsConsumer>();
				//x.AddConsumer<SeatsBookedConsumer>();

				x.AddConsumer<ReserveSeatsConsumer>();
				x.AddConsumer<SeatsReservedConsumer>();

                x.AddConsumer<UnReserveSeatsConsumer>();

                x.AddConsumer<RequestPaymentConsumer>();

                x.AddConsumer<StartBookSeatsSagaConsumer>();
                x.AddConsumer<SeatsBookedSagaConsumer>();
                x.AddConsumer<PaymentAcceptedSagaConsumer>();
                x.AddConsumer<SeatsAlreadyTakenSagaConsumer>();
                x.AddConsumer<UnReservedSeatsSagaConsumer>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CQRS-ES Workshop", Version = "v1", Description = "Web Api Services for CQRS-ES workshop" });
            });
		}

		public void Configure(IApplicationBuilder app, IHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseHsts();

			app.UseAuthentication();
            app.UseFileServer();
            app.UseRouting();
			app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "documentation/{documentName}/documentation.json";
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/documentation/v1/documentation.json", "CQRS Movie Seats API v1");
            });
		}
	}
}