namespace TeamworkApp.Domain.Entities;

public abstract class Post
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public User Author { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public Post()
    {
        Id = Guid.NewGuid();
    }
}
