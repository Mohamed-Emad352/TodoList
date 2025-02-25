namespace TodoList.Application.Features.TodoItems.Commands.MarkTodoItemCommand;

public record MarkTodoItemCommand : IRequest<bool>
{
    public required Guid Id { get; init; }
}