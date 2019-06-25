using System;
using System.Threading.Tasks;
using CqrsMovie.Messages.Dtos;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Muflone.Eventstore;
using Muflone.Eventstore.Persistence;

namespace CqrsMovie.Website.Infrastructure.MongoDb.Readmodel
{
  public class EventStorePositionRepository : IEventStorePositionRepository
  {
    private readonly ILogger<EventStorePositionRepository> logger;
    private readonly IMongoDatabase database;
    public EventStorePositionRepository(ILogger<EventStorePositionRepository> logger, string connectionString)
    {
      this.logger = logger;
      BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
      var client = new MongoClient(connectionString);
      database = client.GetDatabase("CqrsMovie_EventStore_Position"); //Best to inject a class with all parameter and not being coupled like this
    }

    public async Task<IEventStorePosition> GetLastPosition()
    {
      try
      {
        var collection = database.GetCollection<LastEventPosition>(typeof(LastEventPosition).Name);
        var filter = Builders<LastEventPosition>.Filter.Eq("_id", Constants.LastEventPositionKey);
        var result = await collection.CountDocumentsAsync(filter) > 0 ? (await collection.FindAsync(filter)).First() : null;
        if (result == null)
        {
          result = new LastEventPosition
          {
            Id = Constants.LastEventPositionKey,
            CommitPosition = -1,
            PreparePosition = -1
          };
          await collection.InsertOneAsync(result);
        }
        return new EventStorePosition(result.CommitPosition, result.PreparePosition);
      }
      catch (Exception e)
      {
        logger.LogError($"EventStorePositionRepository: Error getting LastSavedPostion, Message: {e.Message}, StackTrace: {e.StackTrace}");
        throw;
      }
    }

    public async Task Save(IEventStorePosition position)
    {
      try
      {
        var collection = database.GetCollection<LastEventPosition>(typeof(LastEventPosition).Name);
        var filter = Builders<LastEventPosition>.Filter.Eq("_id", Constants.LastEventPositionKey);
        var entity = await collection.Find(filter).FirstOrDefaultAsync();
        if (entity == null)
        {
          entity = new LastEventPosition { Id = Constants.LastEventPositionKey, CommitPosition = position.CommitPosition, PreparePosition = position.PreparePosition };
          await collection.InsertOneAsync(entity);
        }
        else
        {
          if (position.CommitPosition > entity.CommitPosition && position.PreparePosition > entity.PreparePosition)
          {
            entity.CommitPosition = position.CommitPosition;
            entity.PreparePosition = position.PreparePosition;
            await collection.FindOneAndReplaceAsync(filter, entity);
          }
        }
      }
      catch (Exception e)
      {
        logger.LogError($"EventStorePositionRepository: Error while updating commit position: {e.Message}, StackTrace: {e.StackTrace}");
        throw;
      }
    }
  }
}
