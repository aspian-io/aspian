using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.CommentModel
{
    public class CommentHistoryConfig : IEntityTypeConfiguration<CommentHistory>
    {
        public void Configure(EntityTypeBuilder<CommentHistory> builder)
        {
            builder
                .HasOne(ch => ch.CreatedBy)
                .WithMany(u => u.CreatedCommentHistories)
                .HasForeignKey(ch => ch.CreatedById);

        }
    }
}