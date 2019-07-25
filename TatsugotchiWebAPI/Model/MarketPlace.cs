using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TatsugotchiWebAPI.Model{
    public class MarketPlace {
        private static MarketPlace market;

        public ICollection<Listing> Animals { get; set; }

        private MarketPlace(){
            Animals = new List<Listing>();
        }

        public static MarketPlace MakeMarket() {
            if (market == null)
                market = new MarketPlace();

            return market;
        }
    }
}
