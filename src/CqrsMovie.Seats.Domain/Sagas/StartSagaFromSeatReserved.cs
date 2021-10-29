﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CqrsMovie.Messages.Commands.Payment;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Dtos;
using CqrsMovie.Messages.Events.Payment;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Seats.ReadModel.Services.Abstracts;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone;
using Muflone.Messages.Commands;
using Muflone.Messages.Events;
using Timer = System.Timers.Timer;

namespace CqrsMovie.Seats.Domain.Sagas
{
    public sealed class StartSagaFromSeatReserved : ICommandHandler<StartSagaFromSeatsReserved>,
        IDomainEventHandler<SeatsReserved>,
        IDomainEventHandler<SeatsBooked>,
        IDomainEventHandler<SeatsFreed>,
        IDomainEventHandler<SeatsAlreadyFreed>,
        IDomainEventHandler<PaymentAccepted>
    {
        private readonly IServiceBus serviceBus;
        private readonly ISeatsService seatsService;

        private readonly Timer timer = new Timer();

        private static readonly Guid CorrelationId = new Guid("c1f4109f-9a97-47b2-91e9-bfce934beed1");

        private static readonly Guid DailyProgramming1 = new Guid("ABD6E805-3C9D-4BE4-9B3F-FB8E22CC9D4A");
        private static readonly Guid DailyProgramming2 = new Guid("613E87B2-CB17-4AB3-85EF-BD78D3C3463C");

        private static readonly IList<Seat> Seats = new List<Seat>
        {
            new Seat { Number = 1, Row = "B" },
            new Seat { Number = 2, Row = "B" },
            new Seat { Number = 3, Row = "B" }
        };

        public StartSagaFromSeatReserved(IServiceBus serviceBus,
            ISeatsService seatsService)
        {
            this.serviceBus = serviceBus;
            this.seatsService = seatsService;
        }

        public Task Handle(StartSagaFromSeatsReserved command)
        {
            throw new NotImplementedException();
        }

        public Task Handle(SeatsReserved @event)
        {
            // Send request for CreditCard Authorization
            // Mock it with a delay
            this.timer.Elapsed += this.PaymentRefused;
            this.timer.Interval = 60 * 1000;
            this.timer.Enabled = true;

            return Task.CompletedTask;
        }

        public async Task Handle(PaymentAccepted @event)
        {
            var bookSeats = new BookSeats(@event.AggregateId, CorrelationId, Seats);
            await this.serviceBus.Send(bookSeats);
        }

        public async Task Handle(SeatsBooked @event)
        {
            if (!@event.Headers.CorrelationId.Equals(CorrelationId))
                return;

            this.timer.Enabled = false;

            await this.seatsService.BookSeats(@event);
        }

        public async Task Handle(SeatsFreed @event)
        {
            if (!@event.Headers.CorrelationId.Equals(CorrelationId))
                return;

            await this.seatsService.FreeSeats(@event);
            await ManageCreditCardRefund(@event);
        }

        public Task Handle(SeatsAlreadyFreed @event)
        {
            if (!@event.Headers.CorrelationId.Equals(CorrelationId))
                return Task.CompletedTask;

            return Task.CompletedTask;
        }

        private Task ManageCreditCardRefund(DomainEvent @event)
        {
            if (!@event.Headers.CorrelationId.Equals(CorrelationId))
                return Task.CompletedTask;

            return Task.CompletedTask;
        }

        #region Timer
        private void PaymentRefused(object sender, ElapsedEventArgs e)
        {
            this.timer.Enabled = false;

            var freeSeats = new FreeSeats(new DailyProgrammingId(DailyProgramming1), CorrelationId, Seats);
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

        ~StartSagaFromSeatReserved()
        {
            this.Dispose(false);
        }
        #endregion
    }
}