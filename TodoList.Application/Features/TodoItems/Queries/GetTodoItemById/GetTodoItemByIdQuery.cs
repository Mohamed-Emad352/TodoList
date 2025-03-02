using TodoList.Application.Features.TodoItems.Queries.GetTodoItems;

namespace TodoList.Application.Features.TodoItems.Queries.GetTodoItemById;

public record GetTodoItemByIdQuery : IRequest<TodoItemDto>
{
    public Guid Id { get; init; }
}