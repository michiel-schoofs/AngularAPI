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
            public string Username { get; set; }
            public string Email { get; set; }
            public DateTime BirthDay { get; set; }
            public Image Image { get; set; }
            public bool HasImage { get => Image != null; }
        #endregion

        #region Constructors
            public PetOwner(RegisterDTO rdto){
                BirthDay = rdto.BirthDay;
                Email = rdto.Email;
                Username = rdto.Username;
            }

            //EF Constructor
            protected PetOwner() { } 
        #endregion

    }
}
