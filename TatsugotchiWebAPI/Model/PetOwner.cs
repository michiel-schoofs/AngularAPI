using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            [NotMapped]
            public Animal FavoriteAnimal { get => Animals.FirstOrDefault(a => a.IsFavorite); }
            public ICollection<Animal> Animals { get; set; }
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
            }

            //EF Constructor
            protected PetOwner() { }
        #endregion
    }
}
