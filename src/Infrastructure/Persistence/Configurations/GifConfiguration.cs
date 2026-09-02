using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Infrastructure.Persistence.Configurations;

public class GifConfiguration : IEntityTypeConfiguration<Gif>
{
    public void Configure(EntityTypeBuilder<Gif> builder)
    {
        builder.ToTable("Gifs");
    }
}
