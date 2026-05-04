using Bogus;
using ToDoListChallenge.Application.ViewModels;
using ToDoListChallenge.Application.ViewModels.Enum;

namespace ToDoListChallenge.Tests.FakeData.Task
{
    public class UpdateTaskItemViewModelFaker : Faker<UpdateTaskItemViewModel>
    {
        public UpdateTaskItemViewModelFaker()
        {
            RuleFor(t => t.Id, f => f.Random.Guid());
            RuleFor(t => t.Status, f => f.PickRandom<TaskItemStatus>());
        }
    }
}
