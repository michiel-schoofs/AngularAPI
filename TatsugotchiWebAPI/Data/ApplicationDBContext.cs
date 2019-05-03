using Microsoft.EntityFrameworkCore;
using TatsugotchiWebAPI.Data.Mapping;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Data {
    public class ApplicationDBContext : DbContext {

        public DbSet<Animal> Animals { get; }
        public DbSet<Badge> Badges { get; }

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
                .ApplyConfiguration<Badge>(new BadgesMapper());

            SeedData(modelBuilder);
        }

        public void SeedData(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Badge>().HasData(new Badge[] {
                new Badge(7,"Ieuwww stinky","smoker",0.1,BadgeType.negative,true){ID=1}

            });
        }
    }
}
