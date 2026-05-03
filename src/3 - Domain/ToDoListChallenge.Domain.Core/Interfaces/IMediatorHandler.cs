using ToDoListChallenge.Domain.Core.Commands;
using ToDoListChallenge.Domain.Core.Events;

namespace ToDoListChallenge.Domain.Core.Interfaces
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T @event) where T : Event;
        Task<bool> SendCommandWithBoolResult<T>(T command) where T : Command;
    }
}
