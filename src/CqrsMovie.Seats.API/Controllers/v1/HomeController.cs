using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CqrsMovie.Seats.API.Controllers.v1
{
  public class HomeController : BaseController
  {
    public HomeController(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }

    public ActionResult Index()
    {
      return View();
    }

    public IActionResult Get()
    {
      return Ok();
    }
  }
}