using ToDoListChallenge.Domain.Core.Events;

namespace ToDoListChallenge.Domain.Events
{
    public class TaskRemovedEvent : Event
    {
        public TaskRemovedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
