using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Infrastructure.Persistence.Configurations;

public class FlaggedContentConfiguration : IEntityTypeConfiguration<FlaggedContent>
{
    public void Configure(EntityTypeBuilder<FlaggedContent> builder)
    {
        builder.ToTable("FlaggedContents", t =>
            t.HasCheckConstraint("CK_FlaggedContent_ExactlyOneTarget",
                "(\"PostId\" IS NOT NULL AND \"CommentId\" IS NULL) OR (\"PostId\" IS NULL AND \"CommentId\" IS NOT NULL)"));

        builder.HasOne(f => f.Post)
            .WithMany()
            .HasForeignKey(f => f.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Comment)
            .WithMany()
            .HasForeignKey(f => f.CommentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.FlaggedBy)
            .WithMany()
            .HasForeignKey(f => f.FlaggedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
