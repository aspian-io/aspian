using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.ActivityModel
{
    public class ActivityOccurrencemetaConfig : IEntityTypeConfiguration<ActivityOccurrencemeta>
    {
        public void Configure(EntityTypeBuilder<ActivityOccurrencemeta> builder)
        {
            builder
                .HasOne(aom => aom.CreatedBy)
                .WithMany(u => u.ActivityOccurrencemetas)
                .HasForeignKey(aom => aom.CreatedById);
        }
    }
}