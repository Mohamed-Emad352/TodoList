namespace TodoList.Application.Features.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(c => c.Title)
            .MaximumLength(50).WithMessage("Maximum title length is 50 characters")
            .NotEmpty();
    }
}