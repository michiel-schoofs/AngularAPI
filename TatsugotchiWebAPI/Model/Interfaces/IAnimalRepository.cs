using System.Collections.Generic;

namespace TatsugotchiWebAPI.Model.Interfaces {
    public interface IAnimalRepository {
        #region Interface Methods
            ICollection<Animal> GetAllAnimals();
            ICollection<Animal> GetNotDeceasedAnimals();
            Animal GetAnimal(int id);
            void RemoveAnimal(Animal animal);
            void AddAnimal(Animal animal,bool isMultithreaded=false);
            void SaveChanges(); 
        #endregion
    }
}
