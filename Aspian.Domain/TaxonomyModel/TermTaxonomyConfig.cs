using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aspian.Domain.TaxonomyModel
{
    public class TermTaxonomyConfig : IEntityTypeConfiguration<TermTaxonomy>
    {
        public void Configure(EntityTypeBuilder<TermTaxonomy> builder)
        {
            var converter = new EnumToStringConverter<TaxonomyEnum>();
            builder
                .Property(tt => tt.Taxonomy)
                .HasConversion(converter);
                
            builder
                .HasOne(tt => tt.CreatedBy)
                .WithMany(u => u.CreatedTermTaxonomies)
                .HasForeignKey(tt => tt.CreatedById);

            builder
                .HasOne(tt => tt.ModifiedBy)
                .WithMany(u => u.ModifiedTermTaxonomies)
                .HasForeignKey(tt => tt.ModifiedById);

            builder
                .HasMany(tt => tt.ChildTaxonomies)
                .WithOne(tt => tt.Parent)
                .HasForeignKey(tt => tt.ParentId);
        }
    }
}