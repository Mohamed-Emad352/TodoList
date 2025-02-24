using TodoList.Application.Common.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Application.Features.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandHandler: IRequestHandler<CreateTodoItemCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = new TodoItem
        {
            Title = request.Title,
            Description = request.Description
        };

        await _context.TodoItems.AddAsync(todoItem, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return todoItem.Id;
    }
}