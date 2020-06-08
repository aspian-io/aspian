using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.AttachmentModel
{
    public class AttachmentmetaConfig : IEntityTypeConfiguration<Attachmentmeta>
    {
        public void Configure(EntityTypeBuilder<Attachmentmeta> builder)
        {
            builder
                .HasOne(am => am.CreatedBy)
                .WithMany(u => u.CreatedAttachmentmetas)
                .HasForeignKey(am => am.CreatedById);
            builder
                .HasOne(am => am.ModifiedBy)
                .WithMany(u => u.ModifiedAttachmentmetas)
                .HasForeignKey(am => am.ModifiedById);
        }
    }
}