using ToDoListChallenge.Domain.Validations;

namespace ToDoListChallenge.Domain.Commands
{
    public class UpdateTaskCommand : TaskCommand
    {
        public UpdateTaskCommand(Guid Id, string Title, string Description, DateOnly? DueDate, ToDoListAPI.Domain.Enums.TaskItemStatus Status)
        {
            this.Id = Id;
            this.Title = Title;
            this.Description = Description;
            this.DueDate = DueDate;
            this.Status = Status;
        }
        public override bool IsValid()
        {
            ValidationResult = new UpdateTaskValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
