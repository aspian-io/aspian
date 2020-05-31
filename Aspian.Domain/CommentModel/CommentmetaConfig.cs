using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.CommentModel
{
    public class CommentmetaConfig : IEntityTypeConfiguration<Commentmeta>
    {
        public void Configure(EntityTypeBuilder<Commentmeta> builder)
        {
            builder
                .HasOne(cm => cm.CreatedBy)
                .WithMany(u => u.CreatedCommentmetas)
                .HasForeignKey(cm => cm.CreatedById);

            builder
                .HasOne(cm => cm.ModifiedBy)
                .WithMany(u => u.ModifiedCommentmetas)
                .HasForeignKey(cm => cm.ModifiedById);
        }
    }
}