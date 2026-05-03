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
                await Bus.RaiseEvent(new DomainNotification(request.MessageType, "The task e-mail has already been taken."));
                return await Task.FromResult(false);
            }

            await _taskItemRepository.Add(task);

            if (Commit())
            {
                await Bus.RaiseEvent(new TaskRegisteredEvent(task.Id, task.Title, task.Description, task.DueDate));
            }

            return await Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Handle(RemoveTaskCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
