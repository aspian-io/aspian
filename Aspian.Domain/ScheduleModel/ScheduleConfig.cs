using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aspian.Domain.ScheduleModel
{
    public class ScheduleConfig : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            var scheduleTypeEnumConverter = new EnumToStringConverter<ScheduleTypeEnum>();
            builder
                .Property(s => s.ScheduleType)
                .HasConversion(scheduleTypeEnumConverter);
        }
    }
}