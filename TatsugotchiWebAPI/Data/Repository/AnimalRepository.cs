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
                throw new System.NotImplementedException();
            }

            public Animal GetAnimal(int id) {
                throw new System.NotImplementedException();
            }

            public void RemoveAnimal(Animal animal) {
                throw new System.NotImplementedException();
            }

            public void SaveChanges() {
                _context.SaveChanges();
            } 
        #endregion
    }
}
