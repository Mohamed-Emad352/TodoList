using TodoList.Application.Common.Interfaces;

namespace TodoList.Application.Features.TodoItems.Queries.GetTodoItems;

public class GetTodoItemsQuery : IPaginatedRequest<TodoItemDto>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}