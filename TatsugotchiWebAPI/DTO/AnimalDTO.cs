using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.DTO
{
    public class AnimalDTO{
        #region Properties
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, 100)]
        public int Speed { get; set; }

        [Required]
        [Range(0, 100)]
        public int Charisma { get; set; }

        [Required]
        [Range(1, 100)]
        public int Hunger { get; set; }

        [Required]
        [Range(1, 100)]
        public int Boredom { get; set; }

        [Required]
        public bool IsDeceased { get; set; }

        [Required]
        public bool RanAway { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public bool CanBreed { get; set; }

        [Required]
        public List<BadgeDTO> Badges { get; set; } 

        [Required]
        public string Owner { get; set; }

        [Required]
        public int Value { get; set; }
        #endregion

        #region Constructor
        public AnimalDTO(Animal animal){
                ID = animal.ID;
                Name = animal.Name;

                Speed = animal.Speed;
                Charisma = animal.Charisma;

                Hunger = animal.Hunger;
                Boredom = animal.Boredom;

                IsDeceased = animal.IsDeceased;
                RanAway = animal.RanAway;

                Type = Enum.GetName(typeof(AnimalType), animal.Type);
                Gender = Enum.GetName(typeof(AnimalGender), animal.Gender);

                Age = animal.Age;
                CanBreed = animal.CanBreed;

                Badges = animal.Badges.Select(b => new BadgeDTO(b)).ToList();
                Owner = animal.Owner.Username;

                Value = animal.AnimalValue;
            } 
        #endregion
    }
}
