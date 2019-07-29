using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.DTO
{
    public class MarketListingDTO {
        public int Quantity { get; set; }
        public int ListingAmount { get; set; }
        public ItemDTO Item {get;set;}

        public MarketListingDTO(MarketListing list){
            Quantity = list.Quantity;
            ListingAmount = list.ListingAmount;
            Item = new ItemDTO(list.Item);
        }
    }
}
