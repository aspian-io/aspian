using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.TaxonomyModel
{
    public class TaxonomyPostConfig : IEntityTypeConfiguration<TaxonomyPost>
    {
        public void Configure(EntityTypeBuilder<TaxonomyPost> builder)
        {
            builder.HasKey(tp => new { tp.TaxonomyId, tp.PostId });
            builder
                .HasOne(tp => tp.Taxonomy)
                .WithMany(t => t.TaxonomyPosts)
                .HasForeignKey(tp => tp.TaxonomyId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(tp => tp.Post)
                .WithMany(p => p.TaxonomyPosts)
                .HasForeignKey(tp => tp.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(tp => tp.CreatedBy)
                .WithMany(u => u.CreatedTaxonomyPosts)
                .HasForeignKey(tp => tp.CreatedById);
            builder
                .HasOne(tp => tp.ModifiedBy)
                .WithMany(u => u.ModifiedTaxonomyPosts)
                .HasForeignKey(tp => tp.ModifiedById);
        }
    }
}