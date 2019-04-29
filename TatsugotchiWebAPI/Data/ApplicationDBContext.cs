using Microsoft.EntityFrameworkCore;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.Data {
    public class ApplicationDBContext : DbContext {

        public DbSet<Animal> Animals { get; }
        public DbSet<Badge> Badges { get; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);

            //Make sure the database is created only to be used in development
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
