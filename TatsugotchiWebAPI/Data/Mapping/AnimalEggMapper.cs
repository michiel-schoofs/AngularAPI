using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Data.Mapping
{
    public class AnimalEggMapper : IEntityTypeConfiguration<AnimalEgg>
    {
        public void Configure(EntityTypeBuilder<AnimalEgg> builder)
        {
            builder.ToTable("AnimalEgg");
            builder.HasKey(ae => new { ae.AnID, ae.EggID });

            builder.HasOne(ae => ae.Egg).WithMany(e => e.AnimalEggs)
                   .IsRequired(true).HasForeignKey(ae => ae.EggID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
