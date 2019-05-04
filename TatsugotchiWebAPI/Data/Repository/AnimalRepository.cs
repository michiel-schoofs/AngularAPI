using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Data.Repository {
    public class AnimalRepository : IAnimalRepository {
        private ApplicationDBContext _context;
        private DbSet<Animal> _animals;

        #region Constructor
            public AnimalRepository(ApplicationDBContext context) {
                _context = context;
                _animals = context.Animals;
            } 
        #endregion

        #region Interface Methods
            public void AddAnimal(Animal animal) {
                _animals.Add(animal);
                SaveChanges();
            }

            public ICollection<Animal> GetAllAnimals() {
                return _animals.Include(a=>a.AnimalBadges)
                .Include(a=>a.Tussen).Include(a=>a.TussenKinderen).ToList();
            }

            public Animal GetAnimal(int id) {
                return GetAllAnimals().FirstOrDefault(a => a.ID == id);
            }

            public void RemoveAnimal(Animal animal) {
                _animals.Remove(animal);
                SaveChanges();
            }

            public void SaveChanges() {
                _context.SaveChanges();
            } 
        #endregion
    }
}
