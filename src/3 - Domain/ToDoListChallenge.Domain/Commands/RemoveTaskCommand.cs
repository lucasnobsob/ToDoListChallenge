using ToDoListChallenge.Domain.Validations;

namespace ToDoListChallenge.Domain.Commands
{
    public class RemoveTaskCommand : TaskCommand
    {
        public RemoveTaskCommand(Guid Id)
        {
            this.Id = Id;
        }
        public override bool IsValid()
        {
            ValidationResult = new RemoveTaskValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
