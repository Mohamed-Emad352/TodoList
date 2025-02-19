namespace TodoList.Application.Common.Models;

public class PaginatedList<T> where T : class
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
    }

    public static async Task<PaginatedList<T>> Create(IQueryable<T> items, int pageNumber, int pageSize)
    {
        var paginatedItems = await items.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(paginatedItems, pageNumber, pageSize, await items.CountAsync());
    }
}