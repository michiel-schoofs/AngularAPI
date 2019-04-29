using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TatsugotchiWebAPI.Model {
    public class Animal {

        #region Attributes
            private readonly static Random rand = new Random(); 
        #endregion

        #region Basic Properties
            public int ID { get; set; }

            [Required]
            [StringLength(50, MinimumLength = 3)]
            public string Name { get; protected set; }

            [Required]
            [Range(1,100)]
            public int Speed { get; protected set; }

            [Required]
            [Range(1, 100)]
            public int Hunger { get; protected set; }

            //Charisma is not trainable or inheritable
            [Required]
            [Range(1, 100)]
            public int Charisma { get; protected set; }


            [Required]
            [Range(1, 100)]
            public int Boredom { get; protected set; }

            [DataType(DataType.DateTime)]
            public DateTime BirthDate { get; protected set; }
        #endregion

        #region Associations
            public List<Badge> Badges { get; set; }
            public List<Animal> Parents { get; set; }
        #endregion

        #region Calculated Attributes
            [NotMapped]
            public string Image { get; protected set; }
        #endregion

        #region Constructors
        public Animal(string name, List<Badge> InitialBadges = null,List < Animal> parents=null) {
            Name = name;
            Parents = parents;
            
            //Attributes
            AttributeInheritance();

            //Badges
            Badges = new List<Badge>();
            //if no parents go for InitialBadges
            BadgeInheritance(InitialBadges);

            BirthDate = DateTime.Now;
            Boredom = 0;
        }



        //EF Constructor
        protected Animal() {}
        #endregion

        #region Methods
        private void BadgeInheritance(List<Badge> init = null) {
            if (Parents != null) {
                Badges = Parents.SelectMany(a => a.Badges)
                    .Where(b => b.CalculateInherit() == true).ToList();

                //if there are no badges after random seed take the first
                if (Badges.Count == 0)
                    Badges.Add(Parents.SelectMany(a => a.Badges)
                    .First());
            }
            else {
                if (init == null)
                    throw new ArgumentException("First generation needs badges");

                Badges = Badges.Where(b => b.CalculateInherit()).ToList();

                if (Badges.Count == 0)
                    Badges.Add(init.First());
            }


        }

        private void AttributeInheritance() {
            //Inhertiance
            if (Parents == null) {
                //Animal is first generation
                Speed = rand.Next(1, 100);
                Charisma = rand.Next(1, 100);
            }
            else {
                //Animal needs to have two parents
                if (Parents.Count != 2)
                    throw new ArgumentException("You can't have less or more then two parents");

                Speed = (int)Parents.Average(a => a.Speed);
                Charisma = (int)Parents.Average(a => a.Charisma);
            }
        } 
        #endregion
    }
}
