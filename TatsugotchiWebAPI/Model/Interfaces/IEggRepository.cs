using System.Collections.Generic;

namespace TatsugotchiWebAPI.Model.Interfaces {
    public interface IEggRepository {
        IEnumerable<Egg> GetEggsInNeedOfHatching();
        void SaveChanges();
        void Delete(Egg egg);
        Egg GetEggWithID(int ID);
        void RemoveEggWithID(int ID);
        ICollection<Egg> GetEggsByPetOwner(PetOwner po);
    }
}
