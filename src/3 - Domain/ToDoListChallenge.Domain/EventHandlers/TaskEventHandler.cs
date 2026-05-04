using MediatR;
using ToDoListChallenge.Domain.Events;

namespace ToDoListChallenge.Domain.EventHandlers
{
    public class TaskEventHandler :
        INotificationHandler<TaskRegisteredEvent>,
        INotificationHandler<TaskUpdatedEvent>,
        INotificationHandler<TaskRemovedEvent>
    {
        public Task Handle(TaskRegisteredEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TaskUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(TaskRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
