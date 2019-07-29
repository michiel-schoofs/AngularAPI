using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.DTO;
using TatsugotchiWebAPI.Model.EFClasses;

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
            public DateTime RedeemedMoney { get; set; }
        #endregion

        #region Associations
            [NotMapped]
            public Animal FavoriteAnimal { get => Animals.FirstOrDefault(a => a.IsFavorite); }
            public ICollection<Animal> Animals { get; set; }
            public Image Image { get; set; }
            public bool HasImage { get => Image != null; }
            public ICollection<PetOwner_Item> POI { get; set; }
            [NotMapped]
            public ICollection<Item> Inventory { get => POI.Select(poi => poi.Item).ToList(); }
        #endregion

        #region Constructors
            public PetOwner(RegisterDTO rdto){
                BirthDay = rdto.BirthDay;
                Email = rdto.Email;
                Username = rdto.Username;
                Animals = new List<Animal>();
                POI = new List<PetOwner_Item>();
                WalletAmount = 2000;
                RedeemedMoney = DateTime.Now.AddDays(-1);
            }

            //EF Constructor
            protected PetOwner() { }
        #endregion

        #region Methods
            public void AddItem(Item item)
            {
                AddItem(item, 1);
            }

            public void AddItem(Item item, int quantity)
            {

                if (quantity <= 0)
                    throw new Exception("You can't add a negative amount or zero of items");

                var poi = POI.FirstOrDefault(p => p.Item == item);
                if (poi == null)
                {
                    POI.Add(new PetOwner_Item(this, item) { Quantity = quantity });
                }
                else
                {
                    poi.Quantity += quantity;
                }
            }

            public void RemoveItem(Item item)
            {
                RemoveItem(item, 1);
            }

            public bool ContainsItem(Item item){
                return Inventory.Contains(item);
            }
            
            public int GetQuantity(Item item){
                var poi = POI.FirstOrDefault(p => p.Item == item);

                if (poi == null)
                    return 0;
                else
                    return poi.Quantity;
            }

            public void RemoveItem(Item item, int quantity)
            {
                var poi = POI.FirstOrDefault(p => p.Item == item);

                if (poi == null)
                    throw new Exception("You don't have this item");

                if (poi.Quantity < quantity)
                    throw new Exception("You don't own enough of this item");

                if (poi.Quantity == quantity)
                    POI.Remove(poi);
                else
                    poi.Quantity -= quantity;
            }
	    #endregion
    }
}
