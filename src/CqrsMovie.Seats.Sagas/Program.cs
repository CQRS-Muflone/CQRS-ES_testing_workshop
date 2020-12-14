using System;
using System.IO;
using System.Threading;
using CqrsMovie.Seats.Sagas.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Muflone.MassTransit.RabbitMQ;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Seats.Sagas
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddScoped<ISagaRepository, InMemorySagaRepository>();
            services.AddScoped<ISerializer, Serializer>();

            services.Configure<ServiceBusOptions>(x => configuration.GetSection("MassTransit:RabbitMQ"));
            var serviceBusOptions = new ServiceBusOptions();
            configuration.GetSection("MassTransit:RabbitMQ").Bind(serviceBusOptions);

            //services.AddMufloneMassTransitWithRabbitMQ(serviceBusOptions, x =>
            //{
            //    x.AddConsumer<StartBookSeatsSagaConsumer>();
            //});

            //services.AddMassTransit(x => x.AddConsumer<StartBookSeatsSagaConsumer>());
            //Action<IRabbitMqHostConfigurator> action = null;
            //services.AddSingleton<IBusControl>((Func<IServiceProvider, IBusControl>)(provider => Bus.Factory.CreateUsingRabbitMq((Action<IRabbitMqBusFactoryConfigurator>)(cfg => {
            //    IRabbitMqHost host = cfg.Host(new Uri(serviceBusOptions.BrokerUrl), action ??= (Action<IRabbitMqHostConfigurator>)(h =>
            //    {
            //        h.Username(serviceBusOptions.Login);
            //        h.Password(serviceBusOptions.Password);
            //    }));
            //    cfg.ReceiveEndpoint(host, serviceBusOptions.QueueName, (Action<IRabbitMqReceiveEndpointConfigurator>)(e =>
            //    {
            //        e.PrefetchCount = (ushort)16;
            //        e.UseMessageRetry((Action<IRetryConfigurator>)(x => x.Interval(2, 100)));
            //        e.LoadFrom(provider);
            //    }));
            //}))));
            //services.AddSingleton<IServiceBus, ServiceBus>();
            //services.AddSingleton<IEventBus, ServiceBus>();

            Console.WriteLine("Waiting for Starting Saga...");

            // wait - not to end
            new AutoResetEvent(false).WaitOne();
        }
    }
}
