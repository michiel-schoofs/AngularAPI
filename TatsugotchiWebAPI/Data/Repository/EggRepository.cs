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
        public void Delete(Egg egg)
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                context.Eggs.Remove(egg);
                context.SaveChanges();

            }
        }

        public ICollection<Egg> GetEggsByPetOwner(PetOwner po){
            return _eggs.Where(e => e.Owner == po).ToList();
        }

        public IEnumerable<Egg> GetEggsInNeedOfHatching() {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                return context.Eggs.Where(e => e.TimeRemaining.Milliseconds <= 0)
                .Include(e => e.AnimalEggs).ThenInclude(ea => ea.An)
                .ThenInclude(m => m.AnimalBadges).ThenInclude(ab => ab.Badge).ToList();
            }
        }

        public Egg GetEggWithID(int ID){
            return _eggs
                   .Include(e=>e.Owner)
                   .Include(e=>e.AnimalEggs)
                        .ThenInclude(ae => ae.An)
                            .ThenInclude(a=>a.AnimalBadges)
                                .ThenInclude(ab => ab.Badge)
                   .FirstOrDefault(e => e.ID == ID) ;
        }

        public void RemoveEgg(Egg egg){
            _eggs.Remove(egg);
        }

        public ICollection<Egg> GetEggsFromAnimalOwnedByUser(Animal an,PetOwner po){
            return _eggs.Include(e => e.AnimalEggs).ThenInclude(ae => ae.An)
                .Where(e => e.Parents.Contains(an) && e.Owner==po).ToList();
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }

        public void AddEgg(Egg egg){
            _eggs.Add(egg);
        }
    }
}
