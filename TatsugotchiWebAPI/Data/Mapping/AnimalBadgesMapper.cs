using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Data.Mapping {
    public class AnimalBadgesMapper : IEntityTypeConfiguration<AnimalBadges> {
        public void Configure(EntityTypeBuilder<AnimalBadges> builder) {
            #region Mapping
                builder.ToTable("Animal_Badges");
                builder.HasKey(ab => new { ab.AnimalID, ab.BadgeID });

                builder.HasOne(ab => ab.Animal)
                    .WithMany(ab => ab.AnimalBadges)
                    .HasForeignKey(ab => ab.AnimalID)
                    .HasPrincipalKey(a=>a.ID)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ab => ab.Badge)
                .WithMany()
                .HasForeignKey(ab => ab.BadgeID)
                .HasPrincipalKey(b => b.ID)
                .OnDelete(DeleteBehavior.Cascade); 
            #endregion
        }
    }
}
