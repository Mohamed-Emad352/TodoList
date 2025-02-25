using TodoList.Application.Common.Exceptions;
using TodoList.Application.Common.Interfaces;

namespace TodoList.Application.Features.TodoItems.Commands.DeleteTodoItem;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FindAsync(request.Id, cancellationToken);

        if (todoItem == null)
        {
            throw new NotFoundException();
        }
        
        _context.TodoItems.Remove(todoItem);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}