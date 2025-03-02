using TodoList.Application.Common.Exceptions;
using TodoList.Application.Common.Interfaces;
using TodoList.Application.Features.TodoItems.Queries.GetTodoItems;

namespace TodoList.Application.Features.TodoItems.Queries.GetTodoItemById;

public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemByIdQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<TodoItemDto> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FindAsync(request.Id, cancellationToken);
        if (todoItem is null)
        {
            throw new NotFoundException();
        }
        var todoItemDto = _mapper.Map<TodoItemDto>(todoItem);
        return todoItemDto;
    }
}