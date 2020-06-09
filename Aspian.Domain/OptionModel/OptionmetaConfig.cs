using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aspian.Domain.OptionModel
{
    public class OptionmetaConfig : IEntityTypeConfiguration<Optionmeta>
    {
        public void Configure(EntityTypeBuilder<Optionmeta> builder)
        {
            var keyConverter = new EnumToStringConverter<KeyEnum>();
            builder
                .Property(om => om.Key)
                .HasConversion(keyConverter);

            var valueConverter = new EnumToStringConverter<ValueEnum>();
            builder
                .Property(om => om.Value)
                .HasConversion(valueConverter);

            var defaultValueConverter = new EnumToStringConverter<ValueEnum>();
            builder
                .Property(om => om.DefaultValue)
                .HasConversion(defaultValueConverter);
        }
    }
}