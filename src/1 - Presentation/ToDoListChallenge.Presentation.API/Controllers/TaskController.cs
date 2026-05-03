using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListChallenge.Application.ViewModels;
using ToDoListChallenge.Application.ViewModels.Enum;
using ToDoListChallenge.Domain.Core.Interfaces;
using ToDoListChallenge.Domain.Core.Notifications;
using ToDoListChallenge.Domain.Interfaces;
using ToDoListChallenge.Infra.CrossCutting.Identity.Authorization;
using ToDoListChallenge.Services.Api.Controllers;

namespace ToDoListChallenge.Presentation.API.Controllers
{
    [Authorize]
    public class TaskController : ApiController
    {
        private readonly ITaskAppService _taskAppService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator,
            ITaskAppService taskService,
            ILogger<TaskController> logger)
            : base(notifications, mediator)
        {
            _taskAppService = taskService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "CanReadTaskData", Roles = Roles.Admin)]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskAppService.GetAllAsync();

            return Response(tasks);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "CanReadTaskData", Roles = Roles.Admin)]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Guid recebido: {@guid}", id);

            var taskItemViewModel = await _taskAppService.GetByIdAsync(id);

            return Response(taskItemViewModel);
        }

        [HttpGet("filter")]
        [Authorize(Policy = "CanReadTaskData", Roles = Roles.Admin)]
        public async Task<IActionResult> GetByFilter(TaskItemStatus? status, DateOnly? dueDate)
        {
            _logger.LogInformation("Filtro recebido: {@filter}", new { status, dueDate });

            var taskItemViewModel = await _taskAppService.GetByFilterAsync(status, dueDate);

            return Response(taskItemViewModel);
        }

        [HttpPost]
        [Authorize(Policy = "CanCreateTaskData", Roles = Roles.Admin)]
        public async Task<IActionResult> Post([FromBody] CreateTaskItemViewModel createTaskItemViewModel)
        {
            _logger.LogInformation("Objeto recebido: {@createTaskItemViewModel}", createTaskItemViewModel);

            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(createTaskItemViewModel);
            }

            await _taskAppService.RegisterAsync(createTaskItemViewModel);

            return Response();
        }

        [HttpPut]
        [Authorize(Policy = "CanUpdateTaskData", Roles = Roles.Admin)]
        public async Task<IActionResult> Put([FromBody] UpdateTaskItemViewModel taskItemViewModel)
        {
            _logger.LogInformation("Objeto recebido: {@taskItemViewModel}", taskItemViewModel);

            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(taskItemViewModel);
            }

            await _taskAppService.UpdateAsync(taskItemViewModel);

            return Response(taskItemViewModel);
        }

        [HttpDelete]
        [Authorize(Policy = "CanRemoveTaskData", Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Guid recebido: {@guid}", id);

            await _taskAppService.RemoveAsync(id);

            return NoContent();
        }
    }
}
