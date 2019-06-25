using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CqrsMovie.Seats.API.Controllers
{
  public abstract class BaseController: Controller
  {
    protected ILogger Logger { get; }

    protected BaseController(ILoggerFactory loggerFactory)
    {
      Logger = loggerFactory.CreateLogger(GetType());
    }
  }
}
