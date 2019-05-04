using Microsoft.EntityFrameworkCore;
using TatsugotchiWebAPI.Data.Mapping;
using TatsugotchiWebAPI.Model;
using System.Linq;
using TatsugotchiWebAPI.Model.EFClasses;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.Data {
    public class ApplicationDBContext : DbContext {

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Badge> Badges { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<Animal>(new AnimalMapper())
                .ApplyConfiguration<ChildParentAnimal>(new ChildParenAnimalMapper())
                .ApplyConfiguration<AnimalBadges>(new AnimalBadgesMapper())
                .ApplyConfiguration<Badge>(new BadgesMapper())
                .ApplyConfiguration<Egg>(new EggMapper());
        }

    }
}
