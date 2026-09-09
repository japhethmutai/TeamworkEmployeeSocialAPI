namespace TeamworkApp.Application.Posts;

public record ArticleResult(Guid Id, string Title, string Content, Guid AuthorId, DateTime CreatedAt);
