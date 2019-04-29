using System.Collections.Generic;

namespace TatsugotchiWebAPI.Model.Interfaces {
    public interface IAnimalRepository {
        #region Interface Methods
            ICollection<Animal> GetAllAnimals();
            Animal GetAnimal(int id);
            void RemoveAnimal(Animal animal);
            void AddAnimal(Animal animal);
            void SaveChanges(); 
        #endregion
    }
}
