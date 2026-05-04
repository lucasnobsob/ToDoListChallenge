using ToDoListAPI.Domain.Enums;
using ToDoListChallenge.Domain.Core.Events;

namespace ToDoListChallenge.Domain.Events
{
    public class TaskUpdatedEvent : Event
    {
        public TaskUpdatedEvent(Guid id, string title, string description, DateOnly? dueDate, TaskItemStatus status)
        {
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
        }

        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }
        public DateOnly? DueDate { get; }
    }
}
