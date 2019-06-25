using CqrsMovie.Core.Enums;
using CqrsMovie.Seats.Infrastructure.MongoDb.Readmodel;
using CqrsMovie.SharedKernel.ReadModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Muflone.Eventstore.Persistence;

namespace CqrsMovie.Seats.Infrastructure.MongoDb
{
  public static class MongoDBHelpers
  {
    public static IServiceCollection AddMongoDB(this IServiceCollection services, string connectionString)
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
        var database = client.GetDatabase("CqrsMovie_Seats_ReadModel"); //Best to inject a class with all parameter and not being coupled like this
        return database;
      });
      services.AddScoped<IPersister, Persister>();
      services.AddSingleton<IEventStorePositionRepository>(x => new EventStorePositionRepository(x.GetService<ILogger<EventStorePositionRepository>>(), connectionString));
      return services;
    }
  }
}
