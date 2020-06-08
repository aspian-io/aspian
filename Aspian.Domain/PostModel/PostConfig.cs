using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aspian.Domain.PostModel
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            var statusConverter = new EnumToStringConverter<PostStatusEnum>();
            builder
                .Property(p => p.PostStatus)
                .HasConversion(statusConverter);

            var typeConverter = new EnumToStringConverter<PostTypeEnum>();
            builder
                .Property(p => p.Type)
                .HasConversion(typeConverter);

            builder
                .HasOne(p => p.CreatedBy)
                .WithMany(u => u.CreatedPosts)
                .HasForeignKey(p => p.CreatedById);

            builder
                .HasOne(p => p.ModifiedBy)
                .WithMany(u => u.ModifiedPosts)
                .HasForeignKey(p => p.ModifiedById);

            builder
                .HasMany(p => p.ChildPosts)
                .WithOne(p => p.Parent)
                .HasForeignKey(p => p.ParentId);

            // Attachment's custom navigation configs
            builder
                .HasMany(p => p.Photos)
                .WithOne(a => a.PhotoOwnerPost)
                .HasForeignKey(a => a.PhotoOwnerPostId);
            builder
                .HasMany(p => p.Videos)
                .WithOne(a => a.VideoOwnerPost)
                .HasForeignKey(a => a.VideoOwnerPostId);
            builder
                .HasMany(p => p.Audios)
                .WithOne(a => a.AudioOwnerPost)
                .HasForeignKey(a => a.AudioOwnerPostId);
            builder
                .HasMany(p => p.Pdfs)
                .WithOne(a => a.PdfOwnerPost)
                .HasForeignKey(a => a.PdfOwnerPostId);
            builder
                .HasMany(p => p.TextFiles)
                .WithOne(a => a.TextFileOwnerPost)
                .HasForeignKey(a => a.TextFileOwnerPostId);
            builder
                .HasMany(p => p.OtherFiles)
                .WithOne(a => a.OtherFileOwnerPost)
                .HasForeignKey(a => a.OtherFileOwnerPostId);
            builder
                .HasMany(p => p.AllAttachments)
                .WithOne(a => a.AttachmentOwnerPost)
                .HasForeignKey(a => a.AttachmentOwnerPostId);
        }
    }
}