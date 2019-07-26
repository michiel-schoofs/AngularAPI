using Microsoft.EntityFrameworkCore;
using System;
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

        //Can be called from request and background worker
        public void AddAnimal(Animal animal, bool isMultithreaded=false) {
            if (isMultithreaded) {

                using(ApplicationDBContext context = new ApplicationDBContext()) {
                    context.Badges.Load();
                    context.Animals.Load();
                    context.Animals.Add(animal);
                    context.SaveChanges();
                }

            }else {
                _animals.Add(animal);
                SaveChanges();
            }
        }

            public ICollection<Animal> GetAllAnimals() {
                return _animals.Include(a => a.AnimalBadges)
                .Include(a => a.Owner).ToList();
            }

            public Animal GetAnimal(int id) {
                return GetAllAnimals().FirstOrDefault(a => a.ID == id);
            }

            public ICollection<Animal> GetNotDeceasedAnimals() {
                return GetAllAnimals().Where(a => !(a.IsDeceased||a.RanAway)).ToList();
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
