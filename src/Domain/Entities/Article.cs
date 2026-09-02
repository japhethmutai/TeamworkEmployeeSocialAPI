namespace TeamworkApp.Domain.Entities;

public class Article : Post
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
