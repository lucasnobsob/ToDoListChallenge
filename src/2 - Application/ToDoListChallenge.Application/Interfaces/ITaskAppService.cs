
using ToDoListChallenge.Application.EventSourcedNormalizers;
using ToDoListChallenge.Application.ViewModels;

namespace ToDoListChallenge.Domain.Interfaces
{
    public interface ITaskAppService
    {
        Task<IEnumerable<TaskItemViewModel>> GetAllAsync();
        Task<IList<TaskHistoryData>> GetAllHistoryAsync(Guid id);
        Task<TaskItemViewModel> GetByFilterAsync(Application.ViewModels.Enum.TaskItemStatus? status, DateOnly? dueDate);
        Task<TaskItemViewModel> GetByIdAsync(Guid id);
        Task RegisterAsync(CreateTaskItemViewModel taskItemViewModel);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(UpdateTaskItemViewModel taskItemViewModel);
    }
}
