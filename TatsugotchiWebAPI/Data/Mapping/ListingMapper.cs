using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.Data.Mapping
{
    public class ListingMapper : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.ToTable("Listings");

            builder.HasKey(l => l.ID);
            builder.Property(l => l.ID).IsRequired().ValueGeneratedOnAdd();

            builder.HasOne(l => l.Animal)
                   .WithOne()
                   .HasPrincipalKey<Animal>(a=>a.ID)
                   .HasForeignKey<Listing>(l=>l.AnimalID)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
