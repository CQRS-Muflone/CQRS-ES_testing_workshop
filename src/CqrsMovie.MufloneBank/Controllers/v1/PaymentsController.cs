using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CqrsMovie.MufloneBank.Controllers.v1
{
    public class PaymentsController : BaseController
    {
        public PaymentsController(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        [HttpPost]
        [Route("")]
        public Task<ActionResult<bool>> ChkAvailability()
        {
            return Task.FromResult(new ActionResult<bool>(true));
        }
    }
}