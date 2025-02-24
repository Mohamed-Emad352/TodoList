using TodoList.Domain.Entities;

namespace TodoList.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}