
using TodoList.Application.Common.Models;

namespace TodoList.Application.Features.TodoItems.Queries.GetTodoItems;

public record GetTodoItemsQuery : PaginatedRequest<TodoItemDto>
{
}