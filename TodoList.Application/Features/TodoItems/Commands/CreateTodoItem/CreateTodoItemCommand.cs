namespace TodoList.Application.Features.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand: IRequest<Guid>
{
    public required string Title { get; init; }
    public string? Description { get; init; }
}