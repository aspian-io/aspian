using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aspian.Domain.AttachmentModel
{
    public class AttachmentConfig : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            var attachmentTypeConverter = new EnumToStringConverter<AttachmentTypeEnum>();
            builder
                .Property(a => a.Type)
                .HasConversion(attachmentTypeConverter);

            var UploadLocationConverter = new EnumToStringConverter<UploadLocationEnum>();
            builder
                .Property(a => a.UploadLocation)
                .HasConversion(UploadLocationConverter);

            builder
                .HasOne(a => a.CreatedBy)
                .WithMany(u => u.CreatedAttachments)
                .HasForeignKey(a => a.CreatedById);
            builder
                .HasOne(a => a.ModifiedBy)
                .WithMany(u => u.ModifiedAttachments)
                .HasForeignKey(a => a.ModifiedById);
        }
    }
}