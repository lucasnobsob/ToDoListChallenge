using ToDoListChallenge.Application.ViewModels.Enum;

namespace ToDoListChallenge.Application.ViewModels
{
    public class CreateTaskItemViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskItemStatus Status { get; set; } = TaskItemStatus.Pendente;
        public DateOnly? DueDate { get; set; }
    }
}
