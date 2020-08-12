using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.DTO {
    public class DisplayListingDTO: ListingDTO {
        public AnimalDTO AnimalDTO { get; set; }

        public DisplayListingDTO(): base() {}
        public DisplayListingDTO(Listing listing) : base(listing) {
            AnimalDTO = new AnimalDTO(listing.Animal);
        }
    }
}
