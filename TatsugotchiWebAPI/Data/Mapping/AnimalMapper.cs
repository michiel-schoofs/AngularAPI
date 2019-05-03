using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.Data.Mapping {
    public class AnimalMapper : IEntityTypeConfiguration<Animal> {
        public void Configure(EntityTypeBuilder<Animal> builder) {
            builder.ToTable("Animal");

            //ID Configuration
            builder.HasKey(a => a.ID);
            builder.Property(a => a.ID).IsRequired().ValueGeneratedOnAdd();

            builder.Property(a => a.Hunger).IsRequired();
            builder.Property(a => a.Name).IsRequired().HasMaxLength(50);

            builder.Property(a => a.Speed).IsRequired();
        }
    }
}
