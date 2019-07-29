using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.DTO
{
    public class PetOwnerDTO
    {
        public int ID { get; set; }
        public int Wallet { get; set; }
        public string Username { get; set; }
        public bool HasRedeemedToday { get; set; }

        public PetOwnerDTO(PetOwner po){
            ID = po.UserID;
            Wallet = po.WalletAmount;
            Username = po.Username;

            HasRedeemedToday = (DateTime.Now.Subtract(po.RedeemedMoney).TotalDays < 1);
        }
    }
}
