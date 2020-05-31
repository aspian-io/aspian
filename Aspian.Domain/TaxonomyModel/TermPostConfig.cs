using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.TaxonomyModel
{
    public class TermPostConfig : IEntityTypeConfiguration<TermPost>
    {
        public void Configure(EntityTypeBuilder<TermPost> builder)
        {
            builder.HasKey(tm => new { tm.TermTaxonomyId, tm.PostId });
            builder
                .HasOne(tp => tp.TermTaxonomy)
                .WithMany(tt => tt.TermPosts)
                .HasForeignKey(tp => tp.TermTaxonomyId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(tp => tp.Post)
                .WithMany(p => p.TermPosts)
                .HasForeignKey(tp => tp.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(tp => tp.CreatedBy)
                .WithMany(u => u.CreatedTermPosts)
                .HasForeignKey(tp => tp.CreatedById);
            builder
                .HasOne(tp => tp.ModifiedBy)
                .WithMany(u => u.ModifiedTermPosts)
                .HasForeignKey(tp => tp.ModifiedById);
        }
    }
}