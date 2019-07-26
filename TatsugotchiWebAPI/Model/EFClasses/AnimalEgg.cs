using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TatsugotchiWebAPI.Model.EFClasses
{
    public class AnimalEgg
    {
        public Animal An { get; set; }
        public int AnID { get; set; }
        public Egg Egg { get; set; }
        public int EggID { get; set; }
    }
}
