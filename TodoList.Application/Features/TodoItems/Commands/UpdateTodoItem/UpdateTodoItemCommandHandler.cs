using TodoList.Application.Common.Exceptions;
using TodoList.Application.Common.Interfaces;

namespace TodoList.Application.Features.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<Guid> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FindAsync(request.Id);

        if (todoItem == null)
        {
            throw new NotFoundException();
        }
        
        todoItem.Title = request.Title;
        todoItem.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
        return todoItem.Id;
    }
}