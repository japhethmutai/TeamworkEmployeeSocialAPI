namespace TeamworkApp.Domain.Entities;

public class Gif : Post
{
    public string Url { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
}
