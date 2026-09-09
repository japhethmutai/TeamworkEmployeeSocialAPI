namespace TeamworkApp.Application.Posts;

public class GifNotFoundException : Exception
{
    public GifNotFoundException(Guid gifId) : base($"Gif with ID '{gifId}' was not found.")
    {
    }
}
