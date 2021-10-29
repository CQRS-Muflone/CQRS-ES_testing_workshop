using System.Collections.Generic;
using Muflone.Core;
using Muflone.Messages.Commands;

namespace CqrsMovie.Messages.Commands.Seat
{
    public sealed class StartSeatsSaga : Command
    {
        public IEnumerable<Dtos.Seat> Seats { get; }

        public StartSeatsSaga(IDomainId aggregateId, IEnumerable<Dtos.Seat> seats)
            : base(aggregateId)
        {
            Seats = seats;
        }
    }
}