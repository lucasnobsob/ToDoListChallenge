using ToDoListAPI.Domain.Enums;
using ToDoListChallenge.Domain.Core.Models;

namespace ToDoListAPI.Domain.Entities;

public class TaskItem : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pendente;
    public DateOnly? DueDate { get; set; }
}
