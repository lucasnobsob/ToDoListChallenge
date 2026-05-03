using ToDoListChallenge.Domain.Core.Commands;
using ToDoListAPI.Domain.Enums;

namespace ToDoListChallenge.Domain.Commands
{
    public abstract class TaskCommand : Command
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskItemStatus Status { get; set; } = TaskItemStatus.Pendente;
        public DateOnly? DueDate { get; set; }
    }
}
