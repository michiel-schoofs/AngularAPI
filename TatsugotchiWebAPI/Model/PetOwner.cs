using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.DTO;

namespace TatsugotchiWebAPI.Model
{
    public class PetOwner{
        #region Properties
            //Unique identifier
            public int UserID { get; set; }
            public int WalletAmount { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public DateTime BirthDay { get; set; }
        #endregion

        #region Associations
            public Animal FavoriteAnimal { get; set; }
            public ICollection<Animal> Animals { get; set; }
            public ICollection<Listing> Listings { get; set; }
            public Image Image { get; set; }
            public bool HasImage { get => Image != null; }
        #endregion

        #region Constructors
            public PetOwner(RegisterDTO rdto){
                BirthDay = rdto.BirthDay;
                Email = rdto.Email;
                Username = rdto.Username;
                Animals = new List<Animal>();
                WalletAmount = 2000;
                Listings = new List<Listing>();
            }

            //EF Constructor
            protected PetOwner() { }
        #endregion

        public void MakeListing(Animal an, bool forAdoption, bool forBreeding) {
            Listing l = new Listing(an, forAdoption, forBreeding);
            Listings.Add(l);
        }

        public bool AnimalInListing(Animal an){
            return Listings.Select(l => l.AnimalID == an.ID).Count() != 0;
        }

        public void MakeListing(Animal an, bool forAdoption, bool forBreeding,
                                int adoptAmount, int breedAmount){
            Listing l = new Listing(an, forAdoption, forBreeding, adoptAmount, breedAmount);
            Listings.Add(l);
        }
    }
}
