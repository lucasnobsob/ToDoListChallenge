using ToDoListAPI.Domain.Entities;
using ToDoListAPI.Domain.Enums;

namespace ToDoListChallenge.Domain.Specifications
{
    public class TaskItemFilterSpecification : BaseSpecification<TaskItem>
    {
        public TaskItemFilterSpecification(TaskItemStatus? status, DateOnly? dueDate) 
            : base(c => (status.HasValue && c.Status == status) || (dueDate.HasValue && c.DueDate == dueDate))
        {
        }
    }
}
