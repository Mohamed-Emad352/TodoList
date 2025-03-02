namespace TodoList.Application.Common.Models;

public abstract record PaginatedRequest<T> :  IRequest<PaginatedList<T>> where T : class
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 5;
}