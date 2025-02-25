using AutoMapper.QueryableExtensions;
using TodoList.Application.Common.Interfaces;
using TodoList.Application.Common.Mappings;
using TodoList.Application.Common.Models;

namespace TodoList.Application.Features.TodoItems.Queries.GetTodoItems;

public class GetTodoItemsQueryHandler : IRequestHandler<GetTodoItemsQuery, PaginatedList<TodoItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TodoItemDto>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var todoItems = await _context.TodoItems
            .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        return todoItems;
    }
}