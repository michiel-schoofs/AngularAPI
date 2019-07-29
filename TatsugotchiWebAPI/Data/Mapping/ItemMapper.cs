using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.Data.Mapping
{
    public class ItemMapper : IEntityTypeConfiguration<Item>{
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");

            builder.HasKey(i => i.ID);

            builder.Property(i => i.ID).ValueGeneratedOnAdd();
        }
    }
}
