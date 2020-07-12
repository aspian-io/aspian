using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.PostModel
{
    public class PostAttachmentConfig : IEntityTypeConfiguration<PostAttachment>
    {
        public void Configure(EntityTypeBuilder<PostAttachment> builder)
        {
            builder.HasKey(pa => new { pa.PostId, pa.AttachmentId });
            builder
                .HasOne(pa => pa.Post)
                .WithMany(p => p.PostAttachments)
                .HasForeignKey(pa => pa.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(pa => pa.Attachment)
                .WithMany(a => a.PostAttachments)
                .HasForeignKey(pa => pa.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(pa => pa.CreatedBy)
                .WithMany(u => u.CreatedPostAttachments)
                .HasForeignKey(pa => pa.CreatedById);

            builder
                .HasOne(pa => pa.ModifiedBy)
                .WithMany(u => u.ModifiedPostAttachments)
                .HasForeignKey(pa => pa.ModifiedById);
        }
    }
}