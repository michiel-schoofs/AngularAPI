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
            optionsBuilder.UseSqlServer("Server=tcp:tatsugotchi.database.windows.net,1433;Initial Catalog=Tatsugotchi;Persist Security Info=False;User ID=Sandra;Password=HQbBt52hbzj2L0;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3000;");
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
