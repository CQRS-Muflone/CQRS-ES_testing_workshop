using System;
using System.Collections.Generic;
using System.Linq;
using CqrsMovie.Core.Enums;
using CqrsMovie.Messages.Events.Payment;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Core;

namespace CqrsMovie.Seats.Domain.Entities
{
	public class DailyProgramming : AggregateRoot
	{
		private MovieId movieId;
		private ScreenId screenId;
		private DateTime date;
		private IList<Seat> seats;

		//TODO: Implement user information (due to online shopping)
		//private Guid userId;

		protected DailyProgramming()
		{ }

		public static DailyProgramming CreateDailyProgramming(DailyProgrammingId aggregateId, MovieId movieId,
				ScreenId screenId, DateTime date, IEnumerable<Messages.Dtos.Seat> freeSeats, string movieTitle,
				string screenName)
		{
			return new DailyProgramming(aggregateId, movieId, screenId, date, freeSeats, movieTitle, screenName);
		}

		private DailyProgramming(DailyProgrammingId aggregateId, MovieId movieId, ScreenId screenId, DateTime date,
				IEnumerable<Messages.Dtos.Seat> freeSeats, string movieTitle, string screenName)
		{
			//Null checks etc. ....

			RaiseEvent(new DailyProgrammingCreated(aggregateId, movieId, screenId, date, freeSeats, movieTitle, screenName));
		}

		private void Apply(DailyProgrammingCreated @event)
		{
			Id = @event.AggregateId;
			movieId = @event.MovieId;
			screenId = @event.ScreenId;
			date = @event.Date;
			seats = @event.Seats.ToEntity(SeatState.Free);
		}

		#region BookSeats
		internal void BookSeats(DailyProgrammingId aggregateId, Guid correlationId, IEnumerable<Messages.Dtos.Seat> seatsToBook)
		{
			// Chk for seats availability
			var seatsToChk = seats.Where(seat => seatsToBook.Any(book => book.ToEntity(SeatState.Reserved).Equals(seat)));
			if (seatsToChk.Count() != seatsToBook.Count())
			{
				RaiseEvent(new SeatsAlreadyBooked(aggregateId, correlationId, seatsToBook));
				return;
			}

			// Raise event
			RaiseEvent(new SeatsBooked(aggregateId, correlationId, seatsToBook));
		}

		private void Apply(SeatsBooked @event)
		{
			foreach (var seatBooked in @event.Seats)
			{
				var seat = seats.FirstOrDefault(s => s.Equals(seatBooked.ToEntity(SeatState.Reserved)));
				seats.Remove(seat);
				seats.Add(new Seat(seat.Row, seat.Number, SeatState.Booked));
			}
		}

		private void Apply(SeatsAlreadyBooked @event)
		{

		}
		#endregion

		#region ReserveSeats
		internal void ReserveSeat(DailyProgrammingId aggregateId, Guid correlationId, IEnumerable<Messages.Dtos.Seat> seatsToReserve)
		{
			// Chk for seats availability
			var seatsToChk = seats.Where(seat => seatsToReserve.Any(book => book.ToEntity(SeatState.Free).Equals(seat)));
			if (seatsToChk.Count() != seatsToReserve.Count())
			{
				RaiseEvent(new SeatsAlreadyReserved(aggregateId, seatsToReserve));
				return;
			}

			RaiseEvent(new SeatsReserved(aggregateId, correlationId, seatsToReserve));
		}

		private void Apply(SeatsReserved @event)
		{
			foreach (var seatReserved in @event.Seats)
			{
				var seat = seats.FirstOrDefault(s => s.Equals(seatReserved.ToEntity(SeatState.Free)));
				seats.Remove(seat);
				seats.Add(new Seat(seat.Row, seat.Number, SeatState.Reserved));
			}
		}

		private void Apply(SeatsAlreadyReserved @event)
		{ }
		#endregion

		#region FreeSeats
		internal void ReleaseSeats(DailyProgrammingId aggregateId, Guid correlationId, IEnumerable<Messages.Dtos.Seat> seatsToRelease)
		{
			// Chk ...

			RaiseEvent(new SeatsFreed(aggregateId, correlationId, seatsToRelease));
		}

		private void Apply(SeatsFreed @event)
		{
			foreach (var seatReserved in @event.Seats)
			{
				var seat = seats.FirstOrDefault(s => s.Equals(seatReserved.ToEntity(SeatState.Reserved)));
				seats.Remove(seat);
				seats.Add(new Seat(seat.Row, seat.Number, SeatState.Free));
			}
		}
		#endregion

		#region Payment
		internal void AcceptPayment(DailyProgrammingId aggregateId, Guid correlationId)
		{
			RaiseEvent(new PaymentAccepted(aggregateId, correlationId));
		}

		private void Apply(PaymentAccepted @event)
		{ }
		#endregion
	}
}
