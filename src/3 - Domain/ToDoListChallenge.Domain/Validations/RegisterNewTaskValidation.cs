using ToDoListChallenge.Domain.Commands;

namespace ToDoListChallenge.Domain.Validations
{
    public class RegisterNewTaskValidation : TaskValidation<RegisterNewTaskCommand>
    {
        public RegisterNewTaskValidation()
        {
            ValidateDescription();
            ValidateTitle();
            ValidateDueDate();
            ValidateStatus();
        }
    }
}
