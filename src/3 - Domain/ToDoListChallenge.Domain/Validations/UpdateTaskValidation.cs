using ToDoListChallenge.Domain.Commands;

namespace ToDoListChallenge.Domain.Validations
{
    public class UpdateTaskValidation : TaskValidation<UpdateTaskCommand>
    {
        public UpdateTaskValidation()
        {
            ValidateId();
            ValidateDescription();
            ValidateTitle();
            ValidateDueDate();
            ValidateStatus();
        }
    }
}
