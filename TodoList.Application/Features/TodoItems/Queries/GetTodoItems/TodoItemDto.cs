using TodoList.Domain.Entities;

namespace TodoList.Application.Features.TodoItems.Queries.GetTodoItems;

public class TodoItemDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsCompleted { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoItem, TodoItemDto>();
        }
    }
}