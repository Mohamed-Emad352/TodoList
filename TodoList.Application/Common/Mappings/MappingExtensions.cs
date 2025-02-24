using TodoList.Application.Common.Models;

namespace TodoList.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<T>> ToPaginatedList<T>(this IQueryable<T> query, int pageNumber, int pageSize) where T : class
    {
        return PaginatedList<T>.CreatAsync(query, pageNumber, pageSize);
    }
}