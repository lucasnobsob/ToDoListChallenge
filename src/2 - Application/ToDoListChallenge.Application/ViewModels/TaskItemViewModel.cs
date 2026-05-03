using System.Text.Json.Serialization;
using ToDoListChallenge.Application.ViewModels.Enum;

namespace ToDoListChallenge.Application.ViewModels
{
    public class TaskItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskItemStatus Status { get; set; } = TaskItemStatus.Pendente;
        public DateOnly? DueDate { get; set; }
    }
}
