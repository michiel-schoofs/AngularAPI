using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Data.Mapping {
    public class ChildParenAnimalMapper : IEntityTypeConfiguration<ChildParentAnimal> {
        public void Configure(EntityTypeBuilder<ChildParentAnimal> builder) {
            #region Mapping
                builder.ToTable("Child_Parent");
                builder.HasKey(cp => cp.CP);

                builder.Property(cp => cp.CP).IsRequired().ValueGeneratedOnAdd();

                builder.HasOne(p => p.Child)
                    .WithMany(p=>p.Tussen)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(p => p.Parent)
                    .WithMany()
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict); 
            #endregion
        }
    }
}
