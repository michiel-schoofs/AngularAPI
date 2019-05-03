

namespace TatsugotchiWebAPI.Data.Repository {
    public class DataInitializer {
        private ApplicationDBContext _context { get; set; }

        public DataInitializer(ApplicationDBContext context) {
            _context = context;
        }

        public void Seed() {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
    }
}
