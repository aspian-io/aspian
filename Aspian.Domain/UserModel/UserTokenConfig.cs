using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.UserModel
{
    public class UserTokenConfig : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder
                .HasOne(ut => ut.CreatedBy)
                .WithMany(u => u.Tokens)
                .HasForeignKey(ut => ut.CreatedById);
        }
    }
}