using System.Collections.Generic;

namespace TatsugotchiWebAPI.Model.Interfaces {
    public interface IEggRepository {
        IEnumerable<Egg> GetEggsInNeedOfHatching();
        void SaveChanges();
        void Delete(Egg egg);
        Egg GetEggWithID(int ID);
        void RemoveEgg(Egg egg);
        ICollection<Egg> GetEggsFromAnimalOwnedByUser(Animal an, PetOwner po);
        ICollection<Egg> GetEggsByPetOwner(PetOwner po);
    }
}
