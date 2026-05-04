using AutoMapper;
using Moq;
using ToDoListAPI.Domain.Entities;
using ToDoListChallenge.Application.Services;
using ToDoListChallenge.Application.ViewModels;
using ToDoListChallenge.Application.ViewModels.Enum;
using ToDoListChallenge.Domain.Commands;
using ToDoListChallenge.Domain.Core.Events;
using ToDoListChallenge.Domain.Core.Interfaces;
using ToDoListChallenge.Domain.Interfaces;
using ToDoListChallenge.Infra.Data.Repository.EventSourcing;
using ToDoListChallenge.Tests.FakeData.Task;

namespace ToDoListChallenge.Tests.Application.Services
{
    public class TaskAppServiceTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITaskItemRepository> _taskRepositoryMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IEventStoreRepository> _eventStoreRepositoryMock;
        private readonly TaskAppService _taskAppService;

        public TaskAppServiceTest()
        {
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _eventStoreRepositoryMock = new Mock<IEventStoreRepository>();
            _mapperMock = new Mock<IMapper>();
            _taskRepositoryMock = new Mock<ITaskItemRepository>();

            _taskAppService = new TaskAppService(
                _mapperMock.Object,
                _taskRepositoryMock.Object,
                _eventStoreRepositoryMock.Object,
                _mediatorHandlerMock.Object
            );
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllAllocations()
        {
            // Arrange
            var tasks = new TaskItemFaker().Generate(10);
            MockTaskMapping(tasks);

            _taskRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(tasks);

            // Act
            var result = await _taskAppService.GetAllAsync();

            // Assert
            Assert.Equal(10, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTaskItem_WhenTaskExists()
        {
            // Arrange
            var task = new TaskItemFaker().Generate();
            MockTaskMapping();

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(task);

            // Act
            var result = await _taskAppService.GetByIdAsync(task.Id);

            // Assert
            Assert.NotNull(result);
            _taskRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _taskAppService.GetByIdAsync(taskId);

            // Assert
            Assert.Null(result);
            _taskRepositoryMock.Verify(repo => repo.GetByIdAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task GetByFilterAsync_ShouldReturnFilteredTasks()
        {
            // Arrange
            var status = TaskItemStatus.Pendente;
            var dueDate = DateOnly.FromDateTime(DateTime.Now);
            var tasks = new TaskItemFaker().Generate(5);

            MockTaskMapping(tasks);
            _taskRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<ISpecification<TaskItem>>()))
                .ReturnsAsync(tasks);

            // Act
            var result = await _taskAppService.GetByFilterAsync(status, dueDate);

            // Assert
            Assert.NotNull(result);
            _taskRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<ISpecification<TaskItem>>()), Times.Once);
        }

        private void MockTaskMapping(List<TaskItem>? taskItemList = null)
        {
            _mapperMock.Setup(m => m.Map<TaskItemViewModel>(It.IsAny<TaskItem>()))
                .Returns((TaskItem task) => new TaskItemViewModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = (TaskItemStatus)task.Status,
                    DueDate = task.DueDate
                });

            if (taskItemList is not null)
            {
                _mapperMock.Setup(m => m.Map<List<TaskItemViewModel>>(It.IsAny<List<TaskItem>>()))
                    .Returns(taskItemList.Select(e => new TaskItemViewModel
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Description = e.Description,
                        Status = (TaskItemStatus)e.Status,
                        DueDate = e.DueDate
                    }).ToList());
            }
        }
    }
}
