using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TatsugotchiWebAPI.Model
{
    public class MarketListing{
        public int ID { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public int ListingAmount { get; set; }

        //EF Constructor
        protected MarketListing(){}

        public MarketListing(Item item,int quantity){
            Item = item;
            Quantity = quantity;
            ListingAmount = Item.MoneyValue;
        }

        public Item BuyItem(int quantity){
            if (Quantity < quantity)
                throw new Exception("You can't buy more then the store has");

            Quantity -= quantity;

            return Item;
        }


    }
}
