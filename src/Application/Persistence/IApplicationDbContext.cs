using Microsoft.EntityFrameworkCore;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Application.Persistence;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Article> Articles { get; }
    DbSet<Gif> Gifs { get; }
    DbSet<Comment> Comments { get; }
    DbSet<FlaggedContent> FlaggedContents { get; }
    DbSet<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
