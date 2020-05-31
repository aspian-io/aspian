using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.ActivityModel
{
    public class ActivityOccurrenceConfig : IEntityTypeConfiguration<ActivityOccurrence>
    {
        public void Configure(EntityTypeBuilder<ActivityOccurrence> builder)
        {
            builder
                .HasOne(ao => ao.CreatedBy)
                .WithMany(u => u.ActivityOccurrences)
                .HasForeignKey(ao => ao.CreatedById);
        }
    }
}