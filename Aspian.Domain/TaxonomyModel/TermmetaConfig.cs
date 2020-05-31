using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.TaxonomyModel
{
    public class TermmetaConfig : IEntityTypeConfiguration<Termmeta>
    {
        public void Configure(EntityTypeBuilder<Termmeta> builder)
        {
            builder
                .HasOne(tm => tm.CreatedBy)
                .WithMany(u => u.CreatedTermmetas)
                .HasForeignKey(tm => tm.CreatedById);

            builder
                .HasOne(tm => tm.ModifiedBy)
                .WithMany(u => u.ModifiedTermmetas)
                .HasForeignKey(tm => tm.ModifiedById);
        }
    }
}