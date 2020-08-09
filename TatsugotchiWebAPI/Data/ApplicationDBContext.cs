using Microsoft.EntityFrameworkCore;
using TatsugotchiWebAPI.Data.Mapping;
using TatsugotchiWebAPI.Model;
using System.Linq;
using TatsugotchiWebAPI.Model.EFClasses;
using TatsugotchiWebAPI.Model.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TatsugotchiWebAPI.Data {
    public class ApplicationDBContext : IdentityDbContext { 

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Egg> Eggs { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Market> Market { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server=172.12.0.2,1433; User Id=sa;Password=p@ssw0rd;Database=Tatsugotchi");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<Animal>(new AnimalMapper())
                .ApplyConfiguration<AnimalBadges>(new AnimalBadgesMapper())
                .ApplyConfiguration<Badge>(new BadgesMapper())
                .ApplyConfiguration<Egg>(new EggMapper())
                .ApplyConfiguration<PetOwner>(new PetOwnerMapper())
                .ApplyConfiguration<Listing>(new ListingMapper())
                .ApplyConfiguration<PetOwner_Item>(new PetOwnerItemMapper())
                .ApplyConfiguration<Item>(new ItemMapper())
                .ApplyConfiguration<AnimalEgg>(new AnimalEggMapper());
        }

    }
}
