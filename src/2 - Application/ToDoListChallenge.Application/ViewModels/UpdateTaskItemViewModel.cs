using ToDoListChallenge.Application.ViewModels.Enum;

namespace ToDoListChallenge.Application.ViewModels
{
    public class UpdateTaskItemViewModel
    {
        public Guid Id { get; set; }
        public TaskItemStatus Status { get; set; } = TaskItemStatus.Pendente;
    }
}
