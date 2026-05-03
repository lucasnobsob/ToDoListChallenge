using AutoMapper;
using ToDoListChallenge.Application.EventSourcedNormalizers;
using ToDoListChallenge.Application.ViewModels;
using ToDoListChallenge.Application.ViewModels.Enum;
using ToDoListChallenge.Domain.Commands;
using ToDoListChallenge.Domain.Core.Interfaces;
using ToDoListChallenge.Domain.Interfaces;
using ToDoListChallenge.Domain.Specifications;
using ToDoListChallenge.Infra.Data.Repository.EventSourcing;


namespace ToDoListChallenge.Application.Services
{
    public class TaskAppService : ITaskAppService
    {
        private readonly IMapper _mapper;
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public TaskAppService(IMapper mapper, 
            ITaskItemRepository taskItemRepository, 
            IEventStoreRepository eventStoreRepository, 
            IMediatorHandler bus)
        {
            _mapper = mapper;
            _taskItemRepository = taskItemRepository;
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
        }

        public async Task<IEnumerable<TaskItemViewModel>> GetAllAsync()
        {
            var taskItems = await _taskItemRepository.GetAllAsync();
            return _mapper.Map<List<TaskItemViewModel>>(taskItems);
        }

        public async Task<TaskItemViewModel> GetByIdAsync(Guid id)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(id);
            return _mapper.Map<TaskItemViewModel>(taskItem);
        }

        public async Task<TaskItemViewModel> GetByFilterAsync(TaskItemStatus? status, DateOnly? dueDate)
        {
            var specification = new TaskItemFilterSpecification((ToDoListAPI.Domain.Enums.TaskItemStatus?)status, dueDate);
            var taskItem = await _taskItemRepository.GetAll(specification);
            return _mapper.Map<TaskItemViewModel>(taskItem);
        }

        public async Task RegisterAsync(CreateTaskItemViewModel taskItemViewModel)
        {
            var registerCommand = _mapper.Map<RegisterNewTaskCommand>(taskItemViewModel);
            await Bus.SendCommand(registerCommand);
        }

        public async Task UpdateAsync(UpdateTaskItemViewModel taskItemViewModel)
        {
            var updateCommand = _mapper.Map<UpdateTaskCommand>(taskItemViewModel);
            await Bus.SendCommand(updateCommand);
        }

        public async Task RemoveAsync(Guid id)
        {
            var removeCommand = new RemoveTaskCommand(id);
            await Bus.SendCommand(removeCommand);
        }

        public async Task<IList<TaskHistoryData>> GetAllHistoryAsync(Guid id)
        {
            var storedEvents = await _eventStoreRepository.All(id);
            return TaskHistory.ToJavaScriptCustomerHistory(storedEvents);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
