using TodoList.Application.Common.Exceptions;
using TodoList.Application.Common.Interfaces;
using TodoList.Application.Features.TodoItems.Commands.DeleteTodoItem;

namespace TodoList.Application.Features.TodoItems.Commands.MarkTodoItemCommand;

public class MarkTodoItemCommandHandler : IRequestHandler<MarkTodoItemCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public MarkTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(MarkTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FindAsync(request.Id, cancellationToken);

        if (todoItem == null)
        {
            throw new NotFoundException();
        }

        todoItem.IsCompleted = !todoItem.IsCompleted;

        await _context.SaveChangesAsync(cancellationToken);

        return todoItem.IsCompleted;
    }
}