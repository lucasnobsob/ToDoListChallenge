using ToDoListChallenge.Domain.Core.Events;
using ToDoListChallenge.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using ToDoListChallenge.Infra.Data.Repository.EventSourcing;

namespace ToDoListChallenge.Infra.Data.Repository.EventSourcing
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly EventStoreSqlContext _context;

        public EventStoreRepository(EventStoreSqlContext context)
        {
            _context = context;
        }

        public async Task<IList<StoredEvent>> All(Guid aggregateId)
        {
            return await (from e in _context.StoredEvent where e.AggregateId == aggregateId select e).ToListAsync();
        }

        public void Store(StoredEvent theEvent)
        {
            _context.StoredEvent.Add(theEvent);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
