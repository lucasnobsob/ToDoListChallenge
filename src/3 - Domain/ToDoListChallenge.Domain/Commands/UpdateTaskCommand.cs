using ToDoListAPI.Domain.Enums;
using ToDoListChallenge.Domain.Validations;

namespace ToDoListChallenge.Domain.Commands
{
    public class UpdateTaskCommand : TaskCommand
    {
        public UpdateTaskCommand(Guid Id, TaskItemStatus Status)
        {
            this.Id = Id;
            this.Status = Status;
        }
        public override bool IsValid()
        {
            ValidationResult = new UpdateTaskValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
