using ToDoListChallenge.Domain.Commands;

namespace ToDoListChallenge.Domain.Validations
{
    public class RemoveTaskValidation : TaskValidation<RemoveTaskCommand>
    {
        public RemoveTaskValidation()
        {
            ValidateId();
        }
    }
}
