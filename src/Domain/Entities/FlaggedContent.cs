namespace TeamworkApp.Domain.Entities;

public enum FlaggedContentStatus
{
    Pending,
    Actioned,
    Dismissed
}
public class FlaggedContent
{
    public Guid Id { get; set; }
    public Guid? PostId { get; set; }
    public Guid? CommentId { get; set; }
    public Guid FlaggedById { get; set; }
    public string Reason { get; set; } = string.Empty;
    public FlaggedContentStatus Status { get; set; } = FlaggedContentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Post? Post { get; set; }
    public Comment? Comment { get; set; }
    public User FlaggedBy { get; set; } = null!;
    public FlaggedContent()
    {
        Id = Guid.NewGuid();
    }
}
