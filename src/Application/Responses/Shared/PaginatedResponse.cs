namespace Application.Responses.Shared;

public record PaginatedResponse<T>(
    IEnumerable<T> Data,
    int PageIndex,
    int PageSize,
    int TotalPages
);