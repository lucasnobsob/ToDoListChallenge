using ToDoListChallenge.Application.Interfaces;
using ToDoListChallenge.Application.ViewModels;
using ToDoListChallenge.Domain.Core.Interfaces;
using ToDoListChallenge.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ToDoListChallenge.Services.Api.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;

        protected ApiController(INotificationHandler<DomainNotification> notifications,
                                IMediatorHandler mediator)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediator = mediator;
        }

        protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();

        protected bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }

        protected new IActionResult Response(object? result = null, bool created = false)
        {
            if (IsValidOperation())
            {
                if (result is IPaginatedSuccessResult paginatedResult)
                {
                    return Ok(new PaginatedSuccessResult<object>(
                        true, 
                        paginatedResult.Data, 
                        paginatedResult.TotalCount
                    ));
                }

                if (created)
                    return Created();

                if (result is null)
                    return NoContent();

                return Ok(new SuccessResult<object>(true, result));
            }

            var notifications = _notifications.GetNotifications();
            if (notifications.Any(x => string.Equals(x.Key, "NotFound", StringComparison.OrdinalIgnoreCase)))
            {
                return NotFound(new ErrorResult<object>(false, notifications.Select(x => x.Value)));
            }

            return BadRequest(new ErrorResult<object>(false, notifications.Select(x => x.Value)));
        }

        protected void NotifyModelStateErrors()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.RaiseEvent(new DomainNotification(code, message));
        }

        protected void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(result.ToString(), error.Description);
            }
        }
    }
}
