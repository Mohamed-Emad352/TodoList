using TodoList.Application.Common.Models;

namespace TodoList.Application.Common.Interfaces;

public interface IPaginatedRequest<T> : IRequest<PaginatedList<T>> where T : class
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}