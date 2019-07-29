using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Data.Mapping
{
    public class PetOwnerItemMapper : IEntityTypeConfiguration<PetOwner_Item>
    {
        public void Configure(EntityTypeBuilder<PetOwner_Item> builder)
        {
            builder.ToTable("PO_Item");

            builder.HasKey(poi => new {poi.ItemID,poi.POID });

            builder
                .HasOne(poi => poi.PO)
                .WithMany(po => po.POI)
                .HasForeignKey(poi => poi.POID)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(poi => poi.Item)
                .WithMany()
                .HasForeignKey(poi => poi.ItemID)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
