using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.UserModel
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasMany(u => u.Photos)
                .WithOne(p => p.PhotoOwner)
                .HasForeignKey(p => p.PhotoOwnerId);
        }
    }
}