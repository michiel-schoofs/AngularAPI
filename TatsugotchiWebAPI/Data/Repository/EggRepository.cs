using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.EFClasses;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Data.Repository {
    public class EggRepository : IEggRepository {
        private readonly DbSet<Egg> _eggs;
        private readonly ApplicationDBContext _context;

        public EggRepository(ApplicationDBContext context) {
            _context = context;
            _eggs = context.Eggs;
        }

        //called from multithreaded enviorment
        public void Delete(Egg egg) {
            using (ApplicationDBContext context = new ApplicationDBContext()) {
                context.Eggs.Remove(egg);
                context.SaveChanges();
            }
        }

        public IEnumerable<Egg> GetEggsInNeedOfHatching() {
            return _eggs.Where(e => e.TimeRemaining.Milliseconds <= 0)
                .Include(e=>e.Mother).ThenInclude(m=> m.AnimalBadges).ThenInclude(ab => ab.Badge)
                .Include(e => e.Father) .ThenInclude(f => f.AnimalBadges).ThenInclude(ab => ab.Badge).ToList();
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }
    }
}
