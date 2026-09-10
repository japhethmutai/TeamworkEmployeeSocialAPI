namespace TeamworkApp.Application.Comments;

public record CommentResult(Guid Id, Guid PostId, string Content, Guid AuthorId, DateTime CreatedAt);
