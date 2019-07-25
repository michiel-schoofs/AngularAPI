using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.DTO
{
    public class BadgeDTO{
        [Required]
        [StringLength(200, MinimumLength = 4)]
        public string Description { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public BadgeDTO(Badge badge){
            Description = badge.Description;
            Name = badge.Name;
            Type = Enum.GetName(typeof(BadgeType),badge.Type);
        }
    }
}
