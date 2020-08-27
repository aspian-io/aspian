using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.CommentModel
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .HasOne(c => c.CreatedBy)
                .WithMany(u => u.CreatedComments)
                .HasForeignKey(c => c.CreatedById);

            builder
                .HasOne(c => c.ModifiedBy)
                .WithMany(u => u.ModifiedComments)
                .HasForeignKey(c => c.ModifiedById);

            builder
                .HasMany(c => c.Replies)
                .WithOne(c => c.Parent)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(s => s.Site)
                .WithMany(c => c.Comments)
                .HasForeignKey(s => s.SiteId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}