using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;

namespace CqrsMovie.Seats.ReadModel.Services.Abstracts
{
    public interface ISeatsService
    {
        Task AddDailyProgrammingAsync(DailyProgrammingCreated @event);
        Task ReserveSeats(SeatsReserved @event);
        Task BookSeats(SeatsBooked @event);

        Task FreeSeats(SeatsFreed @event);
    }
}