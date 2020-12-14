using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Muflone.Saga.Persistence;

//TODO: To implement in the persistence concrete code. Create also a package for MongoDB or RavenDB as an example?
namespace CqrsMovie.Sagas.API.Persistence
{
	public class InMemorySagaRepository : ISagaRepository, IDisposable
	{
		private readonly ISerializer serializer;
		internal static readonly ConcurrentDictionary<Guid, string> Data = new ConcurrentDictionary<Guid, string>();

		public InMemorySagaRepository(ISerializer serializer)
		{
			this.serializer = serializer;
		}

		public async Task<TSagaState> GetById<TSagaState>(Guid correlationId) where TSagaState : class, new()
		{
			if (!Data.TryGetValue(correlationId, out var stateSerialized))
				return default;

			return await this.serializer.Deserialize<TSagaState>(stateSerialized).ConfigureAwait(false);
		}

		public async Task Save<TSagaState>(Guid id, TSagaState sagaState) where TSagaState : class, new()
		{
			var serializedData = await this.serializer.Serialize(sagaState);

			Data[id] = serializedData;
		}

		public Task Complete(Guid correlationId)
		{
			Data.TryRemove(correlationId, out _);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			Data.Clear();
		}
	}
}