using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Seats.Domain.Sagas;
using CqrsMovie.Seats.ReadModel.Services.Abstracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Commands
{
    public class StartSagaFromReserveSeatConsumer : CommandConsumer<StartSeatsSaga>
    {
        private readonly IServiceBus serviceBus;
        private readonly ISeatsService seatsService;

        public StartSagaFromReserveSeatConsumer(IRepository repository, ILoggerFactory loggerFactory,
            IServiceBus serviceBus, ISeatsService seatsService) : base(repository, loggerFactory)
        {
            this.serviceBus = serviceBus;
            this.seatsService = seatsService;
        }

        protected override ICommandHandler<StartSeatsSaga> Handler =>
            new StartSagaFromReserveSeat(this.serviceBus, this.seatsService);
        public override async Task Consume(ConsumeContext<StartSeatsSaga> context)
        {
            using var handler = this.Handler;
            await handler.Handle(context.Message);
        }
    }
}