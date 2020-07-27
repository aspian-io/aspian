using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aspian.Domain.SiteModel
{
    public class SiteConfig : IEntityTypeConfiguration<Site>
    {
        public void Configure(EntityTypeBuilder<Site> builder)
        {
            var converter = new EnumToStringConverter<SiteTypeEnum>();
            builder
                .Property(s => s.SiteType)
                .HasConversion(converter);

            builder
                .HasIndex(s => s.SiteType)
                .IsUnique();

            builder
                .HasOne(s => s.ModifiedBy)
                .WithMany(u => u.ModifiedSites)
                .HasForeignKey(s => s.ModifiedById);
        }
    }
}