using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.Data.Mapping {
    public class BadgesMapper : IEntityTypeConfiguration<Badge> {
        public void Configure(EntityTypeBuilder<Badge> builder) {
            builder.ToTable("Badge");
            builder.HasKey(b => b.ID);

            builder.Property(b => b.ID).IsRequired();
        }
    }
}
