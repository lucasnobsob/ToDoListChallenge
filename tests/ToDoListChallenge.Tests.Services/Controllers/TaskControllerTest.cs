using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using ToDoListChallenge.Application.ViewModels;
using ToDoListChallenge.Domain.Core.Interfaces;
using ToDoListChallenge.Domain.Core.Notifications;
using ToDoListChallenge.Domain.Interfaces;
using ToDoListChallenge.Presentation.API.Controllers;
using ToDoListChallenge.Tests.FakeData.Task;

namespace ToDoListChallenge.Tests.Presentation.Controllers
{
    public class TaskControllerTest
    {
        private readonly Mock<ITaskAppService> _taskAppServiceMock;
        private readonly Mock<DomainNotificationHandler> _notificationHandlerMock;
        private readonly Mock<ILogger<TaskController>> _loggerMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly TaskController _taskController;
        private readonly Guid _userId = Guid.NewGuid();

        public TaskControllerTest()
        {
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, _userId.ToString())
            }));

            _taskAppServiceMock = new Mock<ITaskAppService>();
            _notificationHandlerMock = new Mock<DomainNotificationHandler>();
            _loggerMock = new Mock<ILogger<TaskController>>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();

            _taskController = new TaskController(
                _notificationHandlerMock.Object,
                _mediatorHandlerMock.Object,
                _taskAppServiceMock.Object,
                _loggerMock.Object
            );

            _taskController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = userClaims
                }
            };
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllChairs()
        {
            // Arrange
            var tasks = new TaskItemViewModelFaker().Generate(10);
            _taskAppServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _taskController.GetAll();

            // Assert
            var okObject = Assert.IsType<OkObjectResult>(result);
            var data = okObject.Value as SuccessResult<object>;
            Assert.NotNull(data);
            var returnedTasks = Assert.IsAssignableFrom<List<TaskItemViewModel>>(data.Data);
            Assert.Equal(10, returnedTasks.Count);
        }

        [Fact]
        public async Task GetById_WhenTaskExists_ShouldReturnTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskItemViewModelFaker().Generate();
            task.Id = taskId;
            _taskAppServiceMock.Setup(x => x.GetByIdAsync(taskId)).ReturnsAsync(task);

            // Act
            var result = await _taskController.GetById(taskId);

            // Assert
            var okObject = Assert.IsType<OkObjectResult>(result);
            var data = okObject.Value as SuccessResult<object>;
            Assert.NotNull(data);
            var returnedTask = Assert.IsType<TaskItemViewModel>(data.Data);
            Assert.Equal(taskId, returnedTask.Id);
        }

        [Fact]
        public async Task GetById_WhenTaskDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var notifications = new List<DomainNotification>
            {
                new DomainNotification("NotFound", "Task not found")
            };
            _notificationHandlerMock.Setup(x => x.HasNotifications()).Returns(true);
            _notificationHandlerMock.Setup(x => x.GetNotifications()).Returns(notifications);

            // Act
            var result = await _taskController.GetById(taskId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_WhenValidTask_ShouldReturnCreated()
        {
            // Arrange
            var taskViewModel = new CreateTaskItemViewModelFaker().Generate();
            _notificationHandlerMock.Setup(x => x.HasNotifications()).Returns(false);
            _taskAppServiceMock.Setup(x => x.RegisterAsync(It.IsAny<CreateTaskItemViewModel>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskController.Post(taskViewModel);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Create_WhenInvalidTask_ShouldReturnBadRequest()
        {
            // Arrange
            var taskViewModel = new CreateTaskItemViewModelFaker().Generate();
            var notifications = new List<DomainNotification>
            {
                new DomainNotification("Error", "Invalid task data")
            };
            _notificationHandlerMock.Setup(x => x.HasNotifications()).Returns(true);
            _notificationHandlerMock.Setup(x => x.GetNotifications()).Returns(notifications);

            // Act
            var result = await _taskController.Post(taskViewModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task Update_WhenValidTask_ShouldReturnOk()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskViewModel = new UpdateTaskItemViewModelFaker().Generate();
            taskViewModel.Id = taskId;
            _notificationHandlerMock.Setup(x => x.HasNotifications()).Returns(false);
            _taskAppServiceMock.Setup(x => x.UpdateAsync(It.IsAny<UpdateTaskItemViewModel>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskController.Put(taskViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value as SuccessResult<object>;
            Assert.NotNull(data);
        }

        [Fact]
        public async Task Update_WhenIdMismatch_ShouldReturnBadRequest()
        {
            // Arrange
            var taskViewModel = new UpdateTaskItemViewModelFaker().Generate();
            var notifications = new List<DomainNotification>
            {
                new DomainNotification("NotFound", "Task not found")
            };
            _notificationHandlerMock.Setup(x => x.HasNotifications()).Returns(true);
            _notificationHandlerMock.Setup(x => x.GetNotifications()).Returns(notifications);

            // Act
            var result = await _taskController.Put(taskViewModel);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Update_WhenInvalidTask_ShouldReturnBadRequest()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskViewModel = new UpdateTaskItemViewModelFaker().Generate();
            taskViewModel.Id = taskId;
            var notifications = new List<DomainNotification>
            {
                new DomainNotification("Error", "Invalid task data")
            };
            _notificationHandlerMock.Setup(x => x.HasNotifications()).Returns(true);
            _notificationHandlerMock.Setup(x => x.GetNotifications()).Returns(notifications);

            // Act
            var result = await _taskController.Put(taskViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_WhenTaskExists_ShouldReturnNoContent()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _notificationHandlerMock.Setup(x => x.HasNotifications()).Returns(false);
            _taskAppServiceMock.Setup(x => x.RemoveAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            var result = await _taskController.Delete(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WhenTaskDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var notifications = new List<DomainNotification>
            {
                new DomainNotification("NotFound", "Task not found")
            };
            _notificationHandlerMock.Setup(x => x.HasNotifications()).Returns(true);
            _notificationHandlerMock.Setup(x => x.GetNotifications()).Returns(notifications);

            // Act
            var result = await _taskController.Delete(taskId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
