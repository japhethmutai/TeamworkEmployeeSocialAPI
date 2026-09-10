namespace TeamworkApp.Application.Posts;

public class PostNotFoundException : Exception
{
    public PostNotFoundException(Guid postId) : base($"Post with ID '{postId}' was not found.")
    {
    }
}
