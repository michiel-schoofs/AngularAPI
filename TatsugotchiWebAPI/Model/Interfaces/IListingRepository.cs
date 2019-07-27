using System.Collections.Generic;

namespace TatsugotchiWebAPI.Model.Interfaces
{
    public interface IListingRepository{
        IEnumerable<Listing> GetListingsWithInvalidAnimals();
        IEnumerable<Listing> GetAllListings();
        IEnumerable<Listing> GetListingsByUsers(PetOwner po);
        IEnumerable<Listing> GetListingsNotByUser(PetOwner po);
        Listing GetListingWithID(int id);
        void AddListing(Listing li);
        void RemoveListing(Listing li);
        void SaveChanges();
    }
}
