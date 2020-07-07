using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.TaxonomyModel
{
    public class TermConfig : IEntityTypeConfiguration<Term>
    {
        public void Configure(EntityTypeBuilder<Term> builder)
        {
            builder
                .HasIndex(t => t.Name)
                .IsUnique();

            builder
                .HasIndex(t => t.Slug)
                .IsUnique();

            builder
                .HasOne(t => t.CreatedBy)
                .WithMany(u => u.CreatedTerms)
                .HasForeignKey(t => t.CreatedById);

            builder
                .HasOne(t => t.ModifiedBy)
                .WithMany(u => u.ModifiedTerms)
                .HasForeignKey(t => t.ModifiedById);

            builder
                .HasOne(t => t.TermTaxonomy)
                .WithOne(tt => tt.Term)
                .HasForeignKey<Term>(t => t.TermTaxonomyId);
        }
    }
}