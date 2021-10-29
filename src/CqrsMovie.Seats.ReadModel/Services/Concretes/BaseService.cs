using CqrsMovie.SharedKernel.ReadModel;
using Microsoft.Extensions.Logging;

namespace CqrsMovie.Seats.ReadModel.Services.Concretes
{
    public abstract class BaseService
    {
        protected readonly IPersister Persister;
        protected readonly ILogger Logger;

        protected BaseService(IPersister persister, ILoggerFactory loggerFactory)
        {
            this.Persister = persister;
            this.Logger = loggerFactory.CreateLogger(this.GetType());
        }
    }
}