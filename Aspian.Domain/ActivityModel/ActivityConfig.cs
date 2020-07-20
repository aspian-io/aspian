using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aspian.Domain.ActivityModel
{
    public class ActivityConfig : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            var activitySeverityConverter = new EnumToStringConverter<ActivitySeverityEnum>();
            builder
                .Property(a => a.Severity)
                .HasConversion(activitySeverityConverter);

            var activityObjectConverter = new EnumToStringConverter<ActivityObjectEnum>();
            builder
                .Property(a => a.ObjectName)
                .HasConversion(activityObjectConverter);

            builder
                .HasOne(a => a.CreatedBy)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.CreatedById);
        }
    }
}