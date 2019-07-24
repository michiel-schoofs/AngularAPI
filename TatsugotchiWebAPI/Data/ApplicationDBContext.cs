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
        public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server =.\\SQLEXPRESS; Database = Tatsugotchi; Trusted_Connection = True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<Animal>(new AnimalMapper())
                .ApplyConfiguration<ChildParentAnimal>(new ChildParenAnimalMapper())
                .ApplyConfiguration<AnimalBadges>(new AnimalBadgesMapper())
                .ApplyConfiguration<Badge>(new BadgesMapper())
                .ApplyConfiguration<Egg>(new EggMapper())
                .ApplyConfiguration<PetOwner>(new PetOwnerMapper());
        }

    }
}
