using Microsoft.EntityFrameworkCore;
using TatsugotchiWebAPI.Model;
using System.Linq;
using TatsugotchiWebAPI.Model.Interfaces;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Data.Repository
{
    public class PetOwnerRepository : IPetOwnerRepository
    {
        private readonly ApplicationDBContext _context;
        private DbSet<PetOwner> _users;

        public PetOwnerRepository(ApplicationDBContext context){
            _context = context;
            _users = context.PetOwners;
        }

        public void AddPO(PetOwner user){
            _users.Add(user);
        }

        public bool EmailExists(string email){
            return _users.Select(u => u.Email).Contains(email);
        }

        public PetOwner GetByEmail(string email){
            return _users
                .Include(po => po.Image)
                .Include(po=>po.Animals)
                    .ThenInclude(a => a.AnimalBadges)
                        .ThenInclude(ab => ab.Badge)
                .Include(po=>po.Animals)
                    .ThenInclude(a=>a.AnimalEggs)
                        .ThenInclude(ae => ae.Egg)
                .Include(po=>po.POI)
                    .ThenInclude(poi=>poi.Item)
                .FirstOrDefault(f => f.Email == email);
        }

        public void RemovePO(PetOwner user){
            _users.Remove(user);
        }

        public void SaveChanges(){
            _context.SaveChanges();
        }
    }
}
