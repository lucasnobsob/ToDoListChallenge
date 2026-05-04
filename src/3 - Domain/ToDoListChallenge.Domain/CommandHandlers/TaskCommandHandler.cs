using MediatR;
using ToDoListAPI.Domain.Entities;
using ToDoListAPI.Domain.Enums;
using ToDoListChallenge.Domain.Commands;
using ToDoListChallenge.Domain.Core.Events;
using ToDoListChallenge.Domain.Core.Interfaces;
using ToDoListChallenge.Domain.Core.Notifications;
using ToDoListChallenge.Domain.Events;
using ToDoListChallenge.Domain.Interfaces;

namespace ToDoListChallenge.Domain.CommandHandlers
{
    public class TaskCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewTaskCommand, bool>,
        IRequestHandler<UpdateTaskCommand, bool>,
        IRequestHandler<RemoveTaskCommand, bool>
    {
        ITaskItemRepository _taskItemRepository;
        IMediatorHandler Bus;

        public TaskCommandHandler(IUnitOfWork uow, IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications, 
            ITaskItemRepository taskItemRepository)
            : base(uow, bus, notifications)
        {
            _taskItemRepository = taskItemRepository;
            Bus = bus;
        }

        public async Task<bool> Handle(RegisterNewTaskCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return await Task.FromResult(false);
            }

            var task = new TaskItem
            {
                Description = request.Description,
                Title = request.Title,
                Status = TaskItemStatus.Pendente,
                DueDate = request.DueDate
            };

            if (await _taskItemRepository.GetByTitle(task.Title) is not null)
            {
                await Bus.RaiseEvent(new DomainNotification(request.MessageType, "Já existe uma tarefa com este título."));
                return await Task.FromResult(false);
            }

            await _taskItemRepository.Add(task);

            if (Commit())
            {
                await Bus.RaiseEvent(new TaskRegisteredEvent(task.Id, task.Title, task.Description, task.DueDate));
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return await Task.FromResult(false);
            }

            var task = await _taskItemRepository.GetByIdAsync(request.Id);
            if (task is null)
            {
                await Bus.RaiseEvent(new DomainNotification("NotFound", "Tarefa não encontrada."));
                return await Task.FromResult(false);
            }

            _taskItemRepository.Update(task);

            if (Commit())
            {
                await Bus.RaiseEvent(new TaskUpdatedEvent(task.Id, task.Title, task.Description, task.DueDate, task.Status));
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> Handle(RemoveTaskCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return await Task.FromResult(false);
            }

            var task = await _taskItemRepository.GetByIdAsync(request.Id);
            if (task is null)
            {
                await Bus.RaiseEvent(new DomainNotification("NotFound", "Tarefa não encontrada."));
                return await Task.FromResult(false);
            }

            await _taskItemRepository.Remove(task.Id);

            if (Commit())
            {
                await Bus.RaiseEvent(new TaskRemovedEvent(request.Id));
            }
            return await Task.FromResult(true);
        }
    }
}
