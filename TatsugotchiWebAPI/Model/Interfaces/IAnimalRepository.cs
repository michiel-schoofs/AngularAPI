using System.Collections.Generic;
using TatsugotchiWebAPI.Data;

namespace TatsugotchiWebAPI.Model.Interfaces {
    public interface IAnimalRepository {
        #region Interface Methods
            ICollection<Animal> GetAllAnimals();
            ICollection<Animal> GetNotDeceasedAnimals(ApplicationDBContext context);
            Animal GetAnimal(int id);
            void RemoveAnimal(Animal animal);
            void AddAnimal(Animal animal,bool isMultithreaded=false);
            void SaveChanges(); 
        #endregion
    }
}
