namespace TodoList.Application.Features.TodoItems.Commands.DeleteTodoItem;

public record DeleteTodoItemCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
}