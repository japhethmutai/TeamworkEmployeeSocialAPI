namespace TeamworkApp.Application.Posts;

public record GifResult(Guid Id, string Url, string Caption, Guid AuthorId, DateTime CreatedAt);
