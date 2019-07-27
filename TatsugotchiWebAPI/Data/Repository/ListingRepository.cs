using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Data.Repository
{
    public class ListingRepository : IListingRepository{
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Listing> _listings;

        public ListingRepository(ApplicationDBContext context){
            _context = context;
            _listings = context.Listings;
        }

        public void AddListing(Listing li){
            _listings.Add(li);
        }

        public IEnumerable<Listing> GetAllListings(){
            return _listings.Include(l=>l.Animal).ToList();
        }

        public IEnumerable<Listing> GetListingsByUsers(PetOwner po){
            return _listings.Include(l => l.Animal)
                .Where(l => l.Owner==po).ToList();
        }

        public IEnumerable<Listing> GetListingsNotByUser(PetOwner po){
            return GetAllListings().Except(GetListingsByUsers(po)).ToList();
        }

        public IEnumerable<Listing> GetListingsWithInvalidAnimals(){
            using (ApplicationDBContext db = new ApplicationDBContext()) {
                var listings = db.Listings.Include(l=>l.Animal).ToList();
                return listings.Where(l =>(l.Animal.IsDeceased || l.Animal.RanAway));
            }
        }

        public Listing GetListingWithID(int id){
            return GetAllListings().FirstOrDefault(l => l.ID == id);
        }
        
        public void RemoveListing(Listing li){
            _listings.Remove(li);
        }

        public void SaveChanges(){
            _context.SaveChanges();
        }
    }
}
