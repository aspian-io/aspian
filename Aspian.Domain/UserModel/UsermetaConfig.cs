using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspian.Domain.UserModel
{
    public class UsermetaConfig : IEntityTypeConfiguration<Usermeta>
    {
        public void Configure(EntityTypeBuilder<Usermeta> builder)
        {
            builder
                .HasOne(um => um.CreatedBy)
                .WithMany(u => u.CreatedUsermetas)
                .HasForeignKey(um => um.CreatedById);

            builder
                .HasOne(um => um.ModifiedBy)
                .WithMany(u => u.ModifiedUsermetas)
                .HasForeignKey(um => um.ModifiedById);
        }
    }
}