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
        }
    }
}
