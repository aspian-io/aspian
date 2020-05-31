using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aspian.Domain.OptionModel
{
    public class OptionConfig : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            var converter = new EnumToStringConverter<SectionEnum>();
            builder
                .Property(o => o.Section)
                .HasConversion(converter);
        }
    }
}