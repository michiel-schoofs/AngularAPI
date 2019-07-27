using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.DTO
{
    public class EggDTO
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime DateConceived { get; set; }
        [Required]
        public TimeSpan TimeRemaining { get; set; }

        public EggDTO(Egg egg){
            ID = egg.ID;
            Name = egg.Name;
            Type = Enum.GetName(typeof(AnimalType), egg.Type);
            DateConceived = egg.DateConceived;
            TimeRemaining = egg.TimeRemaining;
        }
    }
}
