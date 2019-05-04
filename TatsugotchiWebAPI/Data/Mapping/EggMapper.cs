using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.Data.Mapping {
    public class EggMapper : IEntityTypeConfiguration<Egg> {
        public void Configure(EntityTypeBuilder<Egg> builder) {
            builder.ToTable("Egg");

            builder.HasKey(e => e.ID);
            builder.Property(e => e.ID)
                .IsRequired().ValueGeneratedOnAdd();

            builder.HasOne(e => e.Mother).WithOne()
                .IsRequired().HasForeignKey<Egg>(e=>e.MotherID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Father).WithOne()
                .IsRequired().HasForeignKey<Egg>(e=>e.FatherID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
