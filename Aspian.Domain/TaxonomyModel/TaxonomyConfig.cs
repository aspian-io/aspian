using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aspian.Domain.TaxonomyModel
{
    public class TaxonomyConfig : IEntityTypeConfiguration<Taxonomy>
    {
        public void Configure(EntityTypeBuilder<Taxonomy> builder)
        {
            var converter = new EnumToStringConverter<TaxonomyEnum>();
            builder
                .Property(tt => tt.Name)
                .HasConversion(converter);
                
            builder
                .HasOne(tt => tt.CreatedBy)
                .WithMany(u => u.CreatedTaxonomies)
                .HasForeignKey(tt => tt.CreatedById);

            builder
                .HasOne(tt => tt.ModifiedBy)
                .WithMany(u => u.ModifiedTaxonomies)
                .HasForeignKey(tt => tt.ModifiedById);

            builder
                .HasMany(tt => tt.ChildTaxonomies)
                .WithOne(tt => tt.Parent)
                .HasForeignKey(tt => tt.ParentId);
        }
    }
}