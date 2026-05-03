using ToDoListChallenge.Domain.Validations;

namespace ToDoListChallenge.Domain.Commands
{
    public class RegisterNewTaskCommand : TaskCommand
    {
        public RegisterNewTaskCommand(string Title, string Description, DateOnly? DueDate, ToDoListAPI.Domain.Enums.TaskItemStatus Status)
        {
            this.Title = Title;
            this.Description = Description;
            this.DueDate = DueDate;
            this.Status = Status;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewTaskValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
