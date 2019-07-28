using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model;

namespace TatsugotchiWebAPI.DTO
{
    public class ListingDTO{
        public int ID { get; set; }
        [Required]
        public int AnimalID { get; set; }
        [Required]
        public bool IsAdoptable { get; set; }
        [Required]
        public bool IsBreedable { get; set; }

        public int AdoptAmount { get; set; }
        public int BreedAmount { get; set; }

        public ListingDTO(){ }

        public ListingDTO(Listing list){
            ID = list.ID;
            AnimalID = list.AnimalID;
            IsAdoptable = list.IsAdoptable;
            IsBreedable = list.IsBreedable;
            AdoptAmount = list.AdoptAmount;
            BreedAmount = list.BreedAmount;
        }
    }
}
