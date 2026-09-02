namespace TeamworkApp.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public Post Post { get; set; } = null!;
    public User Author { get; set; } = null!;

    public Comment()
    {
        Id = Guid.NewGuid();
    }
}
