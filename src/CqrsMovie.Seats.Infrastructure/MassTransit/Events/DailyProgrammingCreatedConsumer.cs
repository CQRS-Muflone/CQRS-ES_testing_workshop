using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Seats.ReadModel.EventHandlers;
using CqrsMovie.Seats.ReadModel.Services.Abstracts;
using CqrsMovie.SharedKernel.ReadModel;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Events
{
	public class DailyProgrammingCreatedConsumer : DomainEventConsumer<DailyProgrammingCreated>
	{
		private readonly ISeatsService seatsService;

		public DailyProgrammingCreatedConsumer(IPersister persister, ILoggerFactory loggerFactory, ISeatsService seatsService) : base(persister, loggerFactory)
		{
			this.seatsService = seatsService;
		}

		protected override IDomainEventHandler<DailyProgrammingCreated> Handler =>
				new DailyProgrammingCreatedDomainEventHandler(Persister, LoggerFactory, this.seatsService);
		public override async Task Consume(ConsumeContext<DailyProgrammingCreated> context)
		{
			using var handler = this.Handler;
			await handler.Handle(context.Message);
		}
	}
}
