using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Model {
    public class Animal {

        #region Attributes
            private readonly static Random rand = new Random(); 
            private readonly List<Animal> parents;
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
            public List<AnimalBadges> AnimalBadges { get; set; }
            [NotMapped]
            public List<Badge> Badges { get => AnimalBadges.Select(ab => ab.Badge).ToList(); }
            public List<ChildParentAnimal> Tussen { get; set; }
            public List<ChildParentAnimal> TussenKinderen { get; set; }
            [NotMapped]
            public List<Animal> Parents { get => Tussen.Select(c => c.Parent).ToList(); }
            [NotMapped]
            public List<Animal> Children { get => TussenKinderen.Select(c => c.Child).ToList(); }
        #endregion

        #region Calculated Attributes
        [NotMapped]
            public string Image { get; protected set; }
        #endregion

        #region Constructors
        public Animal(string name, List<Badge> InitialBadges = null,List < Animal> parents=null) {
            Name = name;
            this.parents = parents;
            
            //Attributes
            AttributeInheritance();

            //Badges
            AnimalBadges = new List<AnimalBadges>();
            //if no parents go for InitialBadges
            BadgeInheritance(InitialBadges);

            BirthDate = DateTime.Now;
            Boredom = 0;

            MakeBetweenTableForSelfManyToMany();
        }



        //EF Constructor
        protected Animal() {}
        #endregion

        #region Methods
        private void BadgeInheritance(List<Badge> init = null) {
            if (parents != null) {
                AnimalBadges = parents.SelectMany(a => a.Badges)
                    .Where(b => b.CalculateInherit() == true)
                    .Select(b=>new AnimalBadges(b,this))
                    .ToList();

                //if there are no badges after random seed take the first
                if (Badges.Count == 0)
                    AnimalBadges.Add(new AnimalBadges(parents.SelectMany(a => a.Badges).First(), this));
            }
            else {
                if (init == null)
                    throw new ArgumentException("First generation needs badges");

                AnimalBadges = Badges.Where(b => b.CalculateInherit())
                    .Select(b=>new AnimalBadges(b,this)).ToList();

                if (Badges.Count == 0)
                    AnimalBadges.Add(new AnimalBadges(init.First(),this));
            }


        }

        private void AttributeInheritance() {
            //Inhertiance
            if (parents == null) {
                //Animal is first generation
                Speed = rand.Next(1, 100);
                Charisma = rand.Next(1, 100);
            }
            else {
                //Animal needs to have two parents
                if (parents.Count != 2)
                    throw new ArgumentException("You can't have less or more then two parents");

                Speed = (int)parents.Average(a => a.Speed);
                Charisma = (int)parents.Average(a => a.Charisma);
            }
        } 

        private void MakeBetweenTableForSelfManyToMany() {
            if (parents != null && parents.Count != 2)
                throw new ArgumentException("An animal can only have two or no parents (First generation)");

            Tussen = parents.Select(p => new ChildParentAnimal(p, this)).ToList();
        }
        #endregion
    }
}
