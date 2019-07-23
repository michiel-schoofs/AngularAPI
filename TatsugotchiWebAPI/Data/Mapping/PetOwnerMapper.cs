using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.Data.Mapping
{
    public class PetOwnerMapper : IEntityTypeConfiguration<PetOwner>
    {
        public void Configure(EntityTypeBuilder<PetOwner> builder)
        {
            builder.ToTable("PetOwners");

            builder.HasKey(po => po.UserID);
            builder.Property(po => po.UserID).IsRequired().ValueGeneratedOnAdd();

            builder.Property(po => po.Username).IsRequired().HasMaxLength(20);
        }
    }
}
