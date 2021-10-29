using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Payment;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Dtos;
using CqrsMovie.SharedKernel.Domain.Ids;
using CqrsMovie.Website.Infrastructure.Orchestrator.Abstracts;
using Microsoft.AspNetCore.Mvc;
using CqrsMovie.Website.Models;
using Microsoft.Extensions.Logging;
using Muflone;

namespace CqrsMovie.Website.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IServiceBus serviceBus;
        private readonly ISeatsOrchestrator seatsOrchestrator;
        private static readonly Guid DailyProgramming1 = new Guid("ABD6E805-3C9D-4BE4-9B3F-FB8E22CC9D4A");
        private static readonly Guid DailyProgramming2 = new Guid("613E87B2-CB17-4AB3-85EF-BD78D3C3463C");

        private static readonly Guid CorrelationId = new Guid("c1f4109f-9a97-47b2-91e9-bfce934beed1");

        public HomeController(ILoggerFactory loggerFactory, IServiceBus serviceBus,
            ISeatsOrchestrator seatsOrchestrator) : base(loggerFactory)
        {
            this.serviceBus = serviceBus;
            this.seatsOrchestrator = seatsOrchestrator;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDailyProgramming()
        {
            //The correct roundtrip would be to load the screen with its seats (I would avoid if possible to lookup for these kind of values in the command handlers)
            var seats = new List<Seat>();
            for (var i = 1; i <= 5; i++)
                for (var j = 0; j < 5; j++)
                    seats.Add(new Seat { Number = i, Row = ((char)(65 + j)).ToString() });
            //To improve would be better to create classes for Ids like MovieId, ScreenId, etc.
            await serviceBus.Send(new CreateDailyProgramming(new DailyProgrammingId(DailyProgramming1), new MovieId(Guid.NewGuid()), new ScreenId(Guid.NewGuid()), DateTime.Today, seats, "The Avengers", "Screen 02"));

            seats = new List<Seat>();
            for (var i = 1; i <= 7; i++)
                for (var j = 0; j < 4; j++)
                    seats.Add(new Seat { Number = i, Row = ((char)(65 + j)).ToString() });
            await serviceBus.Send(new CreateDailyProgramming(new DailyProgrammingId(DailyProgramming2), new MovieId(Guid.NewGuid()), new ScreenId(Guid.NewGuid()), DateTime.Today, seats, "Attila flagello di Dio", "Screen 01"));

            ViewData["Message"] = "CreateDailyProgramming commands sent";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> BookSeats()
        {
            var seats = new List<Seat>
              {
                  new Seat { Number = 1, Row = "B" },
                  new Seat { Number = 2, Row = "B" },
                  new Seat { Number = 3, Row = "B" }
              };
            await serviceBus.Send(new BookSeats(new DailyProgrammingId(DailyProgramming1), Guid.NewGuid(), seats));

            ViewData["Message"] = "BookSeats commands sent";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ReserveSeats()
        {
            var seats = new List<Seat>
              {
                new Seat { Number = 1, Row = "B" },
                new Seat { Number = 2, Row = "B" },
                new Seat { Number = 3, Row = "B" }
              };
            await serviceBus.Send(new ReserveSeats(new DailyProgrammingId(DailyProgramming1), CorrelationId, seats));

            ViewData["Message"] = "ReserveSeats commands sent";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> StartSaga()
        {
            var seats = new List<Seat>
            {
                new Seat { Number = 1, Row = "B" },
                new Seat { Number = 2, Row = "B" },
                new Seat { Number = 3, Row = "B" }
            };
            await this.seatsOrchestrator.StartSagaFromReserveSeats(DailyProgramming1, seats);

            //await this.seatsOrchestrator.StartSeatsSaga(DailyProgramming2, seats);

            ViewData["Message"] = "StartSaga commands sent";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AcceptPayment()
        {
            await this.seatsOrchestrator.StartSagaFromSeatsReserved(DailyProgramming1, CorrelationId);

            ViewData["Message"] = "AcceptPayment commands sent";
            return RedirectToAction("Index");
        }
    }
}
