using System;
using System.Linq;
using System.Threading.Tasks;
using CqrsMovie.Core.Enums;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Seats.ReadModel.Dtos;
using CqrsMovie.Seats.ReadModel.Services.Abstracts;
using CqrsMovie.SharedKernel.ReadModel;
using Microsoft.Extensions.Logging;

namespace CqrsMovie.Seats.ReadModel.Services.Concretes
{
    public sealed class SeatsService : BaseService, ISeatsService
    {
        public SeatsService(IPersister persister,
            ILoggerFactory loggerFactory) : base(persister, loggerFactory)
        {
        }

        public async Task AddDailyProgrammingAsync(DailyProgrammingCreated @event)
        {
            var entity = new DailyProgramming
            {
                Date = @event.Date,
                Id = @event.AggregateId.ToString(),
                ScreenId = @event.ScreenId.ToString(),
                Seats = @event.Seats.ToReadModel(SeatState.Free),
                MovieId = @event.MovieId.ToString(),
                MovieTitle = @event.MovieTitle,
                ScreenName = @event.ScreenName
            };
            await Persister.Insert(entity);
        }

        public async Task ReserveSeats(SeatsReserved @event)
        {
            var bookingDailyProgramming = await Persister.GetBy<DailyProgramming>(@event.AggregateId.ToString());

            if (bookingDailyProgramming == null)
                throw new Exception("Update ReadModel by rewind all events :-)");

            @event.Seats.ToList().ForEach(seat =>
            {
                var seatToUpdate =
                    bookingDailyProgramming.Seats.FirstOrDefault(s =>
                        s.Row.Equals(seat.Row) && s.Number.Equals(seat.Number));

                if (seatToUpdate != null)
                    seatToUpdate.State = SeatState.Reserved;
            });
            await Persister.Update(bookingDailyProgramming);
        }

        public async Task BookSeats(SeatsBooked @event)
        {
            var bookingDailyProgramming = await Persister.GetBy<DailyProgramming>(@event.AggregateId.ToString());

            if (bookingDailyProgramming == null)
                throw new Exception("Update ReadModel by rewind all events :-)");

            @event.Seats.ToList().ForEach(seat =>
            {
                var seatToUpdate =
                    bookingDailyProgramming.Seats.FirstOrDefault(s =>
                        s.Row.Equals(seat.Row) && s.Number.Equals(seat.Number));

                if (seatToUpdate != null)
                    seatToUpdate.State = SeatState.Booked;
            });
            await Persister.Update(bookingDailyProgramming);
        }

        public async Task FreeSeats(SeatsFreed @event)
        {
            var bookingDailyProgramming = await Persister.GetBy<DailyProgramming>(@event.AggregateId.ToString());

            if (bookingDailyProgramming == null)
                throw new Exception("Update ReadModel by rewind all events :-)");

            @event.Seats.ToList().ForEach(seat =>
            {
                var seatToUpdate =
                    bookingDailyProgramming.Seats.FirstOrDefault(s =>
                        s.Row.Equals(seat.Row) && s.Number.Equals(seat.Number));

                if (seatToUpdate != null)
                    seatToUpdate.State = SeatState.Free;
            });
            await Persister.Update(bookingDailyProgramming);
        }
    }
}