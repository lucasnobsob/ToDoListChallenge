using Bogus;
using ToDoListChallenge.Application.ViewModels;
using ToDoListChallenge.Application.ViewModels.Enum;

namespace ToDoListChallenge.Tests.FakeData.Task
{
    public class TaskItemViewModelFaker : Faker<TaskItemViewModel>
    {
        public TaskItemViewModelFaker()
        {
            RuleFor(t => t.Id, f => f.Random.Guid());
            RuleFor(t => t.Title, f => f.Lorem.Sentence(3));
            RuleFor(t => t.Description, f => f.Lorem.Paragraph());
            RuleFor(t => t.Status, f => f.PickRandom<TaskItemStatus>());
            RuleFor(t => t.DueDate, f => DateOnly.FromDateTime(f.Date.Future()));
        }
    }
}
