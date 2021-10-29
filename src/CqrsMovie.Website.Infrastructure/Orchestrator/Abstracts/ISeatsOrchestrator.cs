using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CqrsMovie.Website.Infrastructure.Orchestrator.Abstracts
{
    public interface ISeatsOrchestrator
    {
        Task StartSagaFromReserveSeats(Guid aggregateId, IEnumerable<Messages.Dtos.Seat> seats);
        Task StartSagaFromSeatsReserved(Guid aggregateId, Guid correlationId);
    }
}