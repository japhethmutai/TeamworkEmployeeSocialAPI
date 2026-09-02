namespace TeamworkApp.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid AdminId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string Reason {get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public User Admin { get; set; } = null!;
    public AuditLog()
    {
        Id = Guid.NewGuid();
    }
}
