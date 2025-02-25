namespace TodoList.Application.Features.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(c => c.Title)
            .MaximumLength(50).WithMessage("Maximum title lenght is 50 characters")
            .NotEmpty();
    }
}