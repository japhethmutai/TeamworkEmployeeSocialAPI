namespace TeamworkApp.Application.Common;

public record PagedResult<T>(List<T> Items, string? NextCursor, bool HasMore);
