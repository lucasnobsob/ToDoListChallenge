using FluentValidation;
using ToDoListChallenge.Domain.Commands;

namespace ToDoListChallenge.Domain.Validations
{
    public abstract class TaskValidation<T> : AbstractValidator<T> where T : TaskCommand
    {
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("A tarefa deve ter um identificador válido");
        }

        protected void ValidateTitle()
        {
            RuleFor(c => c.Title)
                .NotEmpty()
                .WithMessage("O título da tarefa é obrigatório")
                .MaximumLength(200)
                .WithMessage("O título da tarefa deve ter no máximo 200 caracteres");
        }

        protected void ValidateDescription()
        {
            RuleFor(c => c.Description)
                .MaximumLength(1000)
                .WithMessage("A descrição da tarefa deve ter no máximo 1000 caracteres")
                .When(c => !string.IsNullOrEmpty(c.Description));
        }

        protected void ValidateDueDate()
        {
            RuleFor(c => c.DueDate)
                .GreaterThan(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("A data de vencimento deve ser uma data futura");
        }

        protected void ValidateStatus()
        {
            RuleFor(c => c.Status)
                .IsInEnum()
                .WithMessage("O status da tarefa deve ser um valor válido");
        }
    }
}
