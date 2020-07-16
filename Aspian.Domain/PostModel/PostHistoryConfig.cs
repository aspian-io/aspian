using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.PostModel
{
    public class PostHistoryConfig : IEntityTypeConfiguration<PostHistory>
    {
        public void Configure(EntityTypeBuilder<PostHistory> builder)
        {
            builder
                .HasOne(ph => ph.CreatedBy)
                .WithMany(u => u.CreatedPostHistories)
                .HasForeignKey(ph => ph.CreatedById);

        }
    }
}