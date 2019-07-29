using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TatsugotchiWebAPI.Model
{
    public class Market {
        private static Market _market;

        public int ID { get; set; }
        public ICollection<MarketListing> Listings {get;set;}
        [NotMapped]
        public ICollection<Item> Items { get => Listings.Select(ml => ml.Item).ToList(); }

        protected Market(){}

        public Market(bool notEf){
            Listings = new List<MarketListing>();
        }

        public void AddListing(MarketListing ml){
            Listings.Add(ml);
        }

        public Item BuyItem(Item item,int quantity){
            var listing = Listings.FirstOrDefault(l => l.Item == item);

            if (listing == null)
                throw new Exception("This listing doesn't exist.");

            var it = BuyItem(item, quantity);
            return it;
        }

        public static Market GetMarket(){
            if (_market == null)
                _market = new Market(false);

            return _market;
        }
    }
}
