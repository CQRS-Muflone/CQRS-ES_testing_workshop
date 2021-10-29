using System;
using System.Collections.Generic;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Dtos;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Seats.Domain.CommandHandlers;
using CqrsMovie.SharedKernel.Domain.Ids;
using Microsoft.Extensions.Logging.Abstractions;
using Muflone.Messages.Commands;
using Muflone.Messages.Events;
using Muflone.SpecificationTests;

namespace CqrsMovie.Seats.Domain.Tests.Entities
{
    public class ReserveSeats_AlreadyTaken : CommandSpecification<ReserveSeats>
    {
        private readonly DailyProgrammingId aggregateId = new DailyProgrammingId(Guid.NewGuid());
        private readonly MovieId movieId = new MovieId(Guid.NewGuid());
        private readonly ScreenId screenId = new ScreenId(Guid.NewGuid());
        private readonly DateTime dailyDate = DateTime.Today;
        private readonly string movieTitle = "rambo";
        private readonly string screenName = "screen 99";
        private readonly IEnumerable<Seat> seats;
        private readonly IEnumerable<Seat> seatsToBook;
        private readonly IEnumerable<Seat> seatsToReserve;

        private readonly Guid correlationId = Guid.NewGuid();

        public ReserveSeats_AlreadyTaken()
        {
            seats = new List<Seat>
            {
                new Seat { Number = 1, Row = "A" },
                new Seat { Number = 2, Row = "A" },
                new Seat { Number = 3, Row = "A" },
                new Seat { Number = 4, Row = "A" },
                new Seat { Number = 1, Row = "B" },
                new Seat { Number = 2, Row = "B" },
                new Seat { Number = 3, Row = "B" },
                new Seat { Number = 4, Row = "B" }
            };

            seatsToBook = new List<Seat>
            {
                new Seat { Number = 1, Row = "B" },
                new Seat { Number = 2, Row = "B" },
                new Seat { Number = 3, Row = "B" }
            };

            seatsToReserve = new List<Seat>
            {
                new Seat { Number = 1, Row = "B" },
                new Seat { Number = 2, Row = "B" },
                new Seat { Number = 3, Row = "B" }
            };
        }

        protected override IEnumerable<DomainEvent> Given()
        {
            yield return new DailyProgrammingCreated(aggregateId, movieId, screenId, dailyDate, seats, movieTitle, screenName);
            yield return new SeatsReserved(aggregateId, seatsToReserve);
            yield return new SeatsBooked(aggregateId, this.correlationId, seatsToBook);
        }

        protected override ReserveSeats When()
        {
            return new ReserveSeats(aggregateId, this.correlationId, seatsToReserve);
        }

        protected override ICommandHandler<ReserveSeats> OnHandler()
        {
            return new ReserveSeatsCommandHandler(Repository, new NullLoggerFactory());
        }

        protected override IEnumerable<DomainEvent> Expect()
        {
            yield return new SeatsAlreadyTaken(this.aggregateId, this.seatsToReserve);
        }
    }
}