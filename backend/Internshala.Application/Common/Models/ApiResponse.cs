using Internshala.Shared.Pagination;

namespace Internshala.Application.Common.Models;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string Message { get; init; } = string.Empty;
    public IReadOnlyList<string> Errors { get; init; } = [];
    public PaginationMeta? Pagination { get; init; }
    public string CorrelationId { get; init; } = string.Empty;

    public static ApiResponse<T> Ok(T data, string message = "Success")
        => new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(string error)
        => new() { Success = false, Errors = [error] };

    public static ApiResponse<T> Paginated(PagedResult<T> result, string message = "Success")
        => new()
        {
            Success = true,
            Data = result.Items is T items ? items : default,
            Message = message,
            Pagination = new PaginationMeta
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage
            }
        };
}

public sealed record PaginationMeta
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public bool HasNextPage { get; init; }
    public bool HasPreviousPage { get; init; }
}
