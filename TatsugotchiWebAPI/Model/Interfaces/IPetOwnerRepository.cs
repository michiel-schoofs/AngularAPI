namespace TatsugotchiWebAPI.Model.Interfaces
{
    public interface IPetOwnerRepository{
        void AddPO(PetOwner user);
        void RemovePO(PetOwner user);
        bool EmailExists(string email);
        void SaveChanges();
    }
}
