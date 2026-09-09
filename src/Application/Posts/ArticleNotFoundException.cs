namespace TeamworkApp.Application.Posts;

public class ArticleNotFoundException : Exception
{
    public ArticleNotFoundException(Guid articleId) : base($"Article with ID '{articleId}' was not found.")
    {
    }
}
