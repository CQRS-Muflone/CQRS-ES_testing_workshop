using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Dtos;
using CqrsMovie.SharedKernel.Domain.Ids;
using CqrsMovie.Website.Infrastructure.Orchestrator.Abstracts;
using Muflone;

namespace CqrsMovie.Website.Infrastructure.Orchestrator.Concrete
{
    public class SeatsOrchestrator : ISeatsOrchestrator
    {
        private readonly IServiceBus serviceBus;

        public SeatsOrchestrator(IServiceBus serviceBus)
        {
            this.serviceBus = serviceBus;
        }

        public async Task StartSagaFromReserveSeats(Guid aggregateId, IEnumerable<Seat> seats)
        {
            await serviceBus.Send(new StartSeatsSaga(new DailyProgrammingId(aggregateId), seats));
        }

        public async Task StartSagaFromSeatsReserved(Guid aggregateId, Guid correlationId)
        {
            await this.serviceBus.Send(new StartSagaFromSeatsReserved(new DailyProgrammingId(aggregateId),
                correlationId));
        }
    }
}