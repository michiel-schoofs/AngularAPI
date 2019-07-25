using System.ComponentModel.DataAnnotations;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.DTO
{
    public class GetListingDTO:ListingDTO{

        [Required]
        public int ListingID { get; set; }
        [Required]
        public int OwnerID { get; set; }

        public GetListingDTO(Listing list):base(list){
            ListingID = list.ID;

        }
    }
}
