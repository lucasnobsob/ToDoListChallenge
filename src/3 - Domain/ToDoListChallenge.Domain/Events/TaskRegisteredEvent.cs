using ToDoListChallenge.Domain.Core.Events;

namespace ToDoListChallenge.Domain.Events
{
    public class TaskRegisteredEvent : Event
    {
        public TaskRegisteredEvent(Guid id, string title, string description, DateOnly? dueDate)
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
