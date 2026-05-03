using ToDoListChallenge.Infra.Data.Mappings;
using ToDoListChallenge.Domain.Core.Events;
using Microsoft.EntityFrameworkCore;

namespace ToDoListChallenge.Infra.Data.Context
{
    public class EventStoreSqlContext : DbContext
    {
        public EventStoreSqlContext(DbContextOptions<EventStoreSqlContext> options) : base(options)
        {
        }

        public DbSet<StoredEvent> StoredEvent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new StoredEventMap());
            modelBuilder.ApplyConfiguration(new TaskItemMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
