using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Dtos;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Seats.ReadModel.Services.Abstracts;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone;
using Muflone.Messages.Commands;
using Muflone.Messages.Events;
using Timer = System.Timers.Timer;

namespace CqrsMovie.Seats.Domain.Sagas
{
    public sealed class DailyProgrammingSaga : ICommandHandler<StartSeatsSaga>,
        IDomainEventHandler<SeatsReserved>,
        IDomainEventHandler<SeatsBooked>,
        IDomainEventHandler<SeatsFreed>
    {
        private readonly IServiceBus serviceBus;
        private readonly ISeatsService seatsService;

        private readonly Timer timer = new Timer();

        private static readonly Guid DailyProgramming1 = new Guid("ABD6E805-3C9D-4BE4-9B3F-FB8E22CC9D4A");
        private static readonly IList<Seat> Seats = new List<Seat>
        {
            new Seat { Number = 1, Row = "B" },
            new Seat { Number = 2, Row = "B" },
            new Seat { Number = 3, Row = "B" }
        };

        public DailyProgrammingSaga(IServiceBus serviceBus,
            ISeatsService seatsService)
        {
            this.serviceBus = serviceBus;
            this.seatsService = seatsService;
        }

        public async Task Handle(StartSeatsSaga command)
        {
            var reserveSeats = new ReserveSeats(command.AggregateId, Seats);
            await this.serviceBus.Send(reserveSeats);
        }

        public async Task Handle(SeatsReserved @event)
        {
            // Send request for CreditCard Authorization
            this.timer.Elapsed += this.TimerElapsed;
            this.timer.Interval = 10 * 1000;
            this.timer.Enabled = true;

            // Create a Delay
            Thread.Sleep(20 * 1000);
            var bookSeats = new BookSeats(@event.AggregateId, Seats);
            await this.serviceBus.Send(bookSeats);
        }

        public async Task Handle(SeatsBooked @event)
        {
            this.timer.Enabled = false;

            await this.seatsService.BookSeats(@event);
        }

        public async Task Handle(SeatsFreed @event)
        {
            await this.seatsService.FreeSeats(@event);
        }

        #region Timer
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.timer.Enabled = false;

            var freeSeats = new FreeSeats(new DailyProgrammingId(DailyProgramming1), Seats);
            this.serviceBus.Send(freeSeats).GetAwaiter().GetResult();
        }
        #endregion

        #region Dispose
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DailyProgrammingSaga()
        {
            this.Dispose(false);
        }
        #endregion
    }
}