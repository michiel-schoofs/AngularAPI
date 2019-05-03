using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Data.Mapping {
    public class ChildParenAnimalMapper : IEntityTypeConfiguration<ChildParentAnimal> {
        public void Configure(EntityTypeBuilder<ChildParentAnimal> builder) {
            #region Mapping
                builder.ToTable("Child_Parent");
                builder.HasKey(cp => new { cp.IDChild, cp.IDParent });

                builder.HasOne(p => p.Child)
                    .WithMany(a => a.TussenKinderen)
                    .IsRequired()
                    .HasForeignKey(t => t.IDChild)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(p => p.Parent)
                    .WithMany(a => a.Tussen)
                    .IsRequired()
                    .HasForeignKey(a => a.IDParent)
                    .OnDelete(DeleteBehavior.Restrict); 
            #endregion
        }
    }
}
