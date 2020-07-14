using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Dtos;
using CqrsMovie.SharedKernel.Domain.Ids;
using Microsoft.AspNetCore.Mvc;
using CqrsMovie.Website.Models;
using Microsoft.Extensions.Logging;
using Muflone;

namespace CqrsMovie.Website.Controllers
{
  public class HomeController : BaseController
  {
    private readonly IServiceBus serviceBus;
    private static readonly Guid DailyProgramming1 = new Guid("ABD6E805-3C9D-4BE4-9B3F-FB8E22CC9D4A");
    private static readonly Guid DailyProgramming2 = new Guid("613E87B2-CB17-4AB3-85EF-BD78D3C3463C");

    public HomeController(ILoggerFactory loggerFactory, IServiceBus serviceBus) : base(loggerFactory)
    {
      this.serviceBus = serviceBus;
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
     //Write create daily programming

      ViewData["Message"] = "CreateDailyProgramming commands sent";
      return RedirectToAction("Index");
    }
    
    [HttpPost]
    public async Task<IActionResult> BookSeats()
    {

      ViewData["Message"] = "BookSeats commands sent";
      return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ReserveSeats()
    {


      ViewData["Message"] = "ReserveSeats commands sent";
      return RedirectToAction("Index");
    }

  }
}
