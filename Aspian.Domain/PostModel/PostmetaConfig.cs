using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.PostModel
{
    public class PostmetaConfig : IEntityTypeConfiguration<Postmeta>
    {
        public void Configure(EntityTypeBuilder<Postmeta> builder)
        {
            builder
                .HasOne(pm => pm.CreatedBy)
                .WithMany(u => u.CreatedPostmetas)
                .HasForeignKey(pm => pm.CreatedById);

            builder
                .HasOne(pm => pm.ModifiedBy)
                .WithMany(u => u.ModifiedPostmetas)
                .HasForeignKey(pm => pm.ModifiedById);
        }
    }
}