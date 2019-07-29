using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TatsugotchiWebAPI.Model.EFClasses
{
    public class PetOwner_Item{
        public PetOwner PO { get; set; }
        public int POID { get; set; }

        public Item Item { get; set; }
        public int ItemID { get; set; }

        public int Quantity { get; set; }

        protected PetOwner_Item(){}

        public PetOwner_Item(PetOwner owner, Item item){
            PO = owner;
            POID = owner.UserID;
            Item = item;
            ItemID = item.ID;
        }
    }
}
