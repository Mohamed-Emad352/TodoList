namespace TodoList.Application.Features.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand : IRequest<Guid>
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
}