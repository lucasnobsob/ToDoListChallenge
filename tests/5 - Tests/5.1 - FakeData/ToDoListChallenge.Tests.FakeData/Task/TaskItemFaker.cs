using Bogus;
using ToDoListAPI.Domain.Entities;
using ToDoListAPI.Domain.Enums;

namespace ToDoListChallenge.Tests.FakeData.Task
{
    public class TaskItemFaker : Faker<TaskItem>
    {
        public TaskItemFaker()
        {
            RuleFor(t => t.Id, f => f.Random.Guid());
            RuleFor(t => t.Title, f => f.Lorem.Sentence(3));
            RuleFor(t => t.Description, f => f.Lorem.Paragraph());
            RuleFor(t => t.Status, f => f.PickRandom<TaskItemStatus>());
            RuleFor(t => t.DueDate, f => DateOnly.FromDateTime(f.Date.Future()));
        }
    }
}
