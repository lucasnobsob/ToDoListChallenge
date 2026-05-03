using ToDoListAPI.Domain.Entities;

namespace ToDoListChallenge.Domain.Interfaces
{
    public interface ITaskItemRepository : IRepository<TaskItem>
    {
        Task<TaskItem?> GetByTitle(string title);
    }
}
