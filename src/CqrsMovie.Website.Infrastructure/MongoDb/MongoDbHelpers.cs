using CqrsMovie.Core.Enums;
using CqrsMovie.SharedKernel.ReadModel;
using CqrsMovie.Website.Infrastructure.MongoDb.Readmodel;
using CqrsMovie.Website.ReadModel.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Muflone.Eventstore.Persistence;

namespace CqrsMovie.Website.Infrastructure.MongoDb
{
  public static class MongoDbHelpers
  {
    public static IServiceCollection AddMongoDb(this IServiceCollection services, string connectionString)
    {
      services.AddSingleton<IMongoDatabase>(x =>
      {
        BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
        BsonClassMap.RegisterClassMap<Enumeration>(cm =>
        {
          cm.SetIsRootClass(true);
          cm.MapMember(m => m.Id);
          cm.MapMember(m => m.Name);
        });
        BsonClassMap.RegisterClassMap<SeatState>(cm =>
        {
          cm.MapCreator(c => new SeatState(c.Id, c.Name));
        });
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("CqrsMovie_Website_ReadModel"); //Best to inject a class with all parameter and not being coupled
        return database;
      });

      services.AddScoped<IPersister, Persister>();
      services.AddScoped<IDailyProgrammingQueries, DailyProgrammingQueries>();
      services.AddSingleton<IEventStorePositionRepository>(x => new EventStorePositionRepository(x.GetService<ILogger<EventStorePositionRepository>>(), connectionString));

      return services; //Return services to allow method chaining
    }
  }
}
