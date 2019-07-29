using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.EFClasses;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.DTO
{
    public class POItemDTO{
        public int ID { get; set; }
        public int MoneyVal { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public string URL { get; set; }
        public string CategoryEnum { get; set; }
        public int Quantity { get; set; }

        public POItemDTO(PetOwner_Item item){
            MoneyVal = item.Item.MoneyValue;
            Name = item.Item.Name;
            Value = item.Item.Value;
            URL = item.Item.URL;
            ID = item.Item.ID;

            Quantity = item.Quantity;
            CategoryEnum = Enum.GetName(typeof(ItemCategory), item.Item.Category);
        }
    }
}
