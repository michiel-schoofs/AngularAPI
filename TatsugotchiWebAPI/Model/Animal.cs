using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TatsugotchiWebAPI.Model.EFClasses;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.Model {
    public class Animal {
        #region Consants
            private static readonly double RunAwayChance = 0.5;
            private static readonly double StarveChance = 1;
        #endregion

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
            public DateTime BirthDate { get; set; }

            public bool IsDeceased { get; set; }
            public bool RanAway { get; set; }        
            public bool IsFavorite { get; set; }
        #endregion

        #region Associations
            public AnimalType Type { get; set; }
            public AnimalGender Gender { get; set; }

            public bool Pregnant { get; set; }

            public List<AnimalBadges> AnimalBadges { get; set; }
            [NotMapped]
            public List<Badge> Badges { get => AnimalBadges.Select(ab => ab.Badge).ToList(); }

            
            public List<AnimalEgg> AnimalEggs { get; set; }    

            public PetOwner Owner { get; set; }
        #endregion

        #region Calculated Attributes
            [NotMapped]
             public int Age { get => (DateTime.Now.Subtract(BirthDate).Days) / 5; }
            [NotMapped]
            public bool IsRightAge { get{
                    var boolean = false;

                    switch (this.Type) {
                        case AnimalType.Alpaca:
                            boolean = Age >= 3;
                            break;
                        default:
                            boolean = Age >= 2;
                            break;
                    }

                    return boolean;
                }
            }

            [NotMapped]
            public bool CanBreed { get => (!Pregnant) && IsRightAge
                && !IsDeceased && !RanAway; }

            [NotMapped]
            public int AnimalValue { get => Badges.Select(b => b.Value).Sum(); }
        #endregion

        #region Constructors
        //First gen constructor
        public Animal(string name, List<Badge> InitialBadges,PetOwner po) {
            SharedAttributes(name);
            BadgeInheritance(InitialBadges);

            var r = rand.Next(0, 101);
            if (r <= 60)
                this.Type = AnimalType.Capybara;
            else {
                if (r > 60 && r < 85)
                    this.Type = AnimalType.Tapir;
                else
                    this.Type = AnimalType.Alpaca;
            }

            Owner = po;
        }

        //Has parents constructor
        public Animal(string name, AnimalType type,List<Animal> parents,PetOwner owner) {
            this.parents = parents;
            this.Type = type;

            SharedAttributes(name);
            BadgeInheritance();

            this.Owner = owner;
        }

        //ForTesting purposes
        public Animal(string name, AnimalType type, AnimalGender gender, DateTime birthday,
            List<Badge> InitialBadges, bool deceased = false, bool ranAway = false,
            bool pregnant = false, int hunger = 0, int boredom = 0, PetOwner po = null) {

            AnimalEggs = new List<AnimalEgg>();
            Name = name;
            Type = type;
            Gender = gender;
            BirthDate = birthday;

            BadgeInheritance(InitialBadges);
            IsDeceased = deceased;
            RanAway = ranAway;

            Pregnant = pregnant;
            Hunger = hunger;
            Boredom = boredom;

            Owner = po;
        }
        
        private void SharedAttributes(string name) {
            Name = name;

            Pregnant = false;

            //Random Gender
            int r = rand.Next(0, 2);
            Gender = (r == 0 ? AnimalGender.Female : AnimalGender.Male);

            AnimalBadges = new List<AnimalBadges>();

            AttributeInheritance();

            BirthDate = DateTime.Now;
            Boredom = 0;

            RanAway = false;
            IsDeceased = false;

            AnimalEggs = new List<AnimalEgg>();
        }

        //EF Constructor
        protected Animal() {}
        #endregion

        #region Methods
        private void BadgeInheritance(List<Badge> init = null) {
            if (parents != null) {
                AnimalBadges = parents.SelectMany(a => a.Badges)
                    .ToHashSet().Where(b => b.CalculateInherit() == true)
                    .Select(b=>new AnimalBadges(b,this))
                    .ToList();

                //if there are no badges after random seed take the first
                if (Badges.Count == 0)
                    AnimalBadges.Add(new AnimalBadges(parents.SelectMany(a => a.Badges).First(), this));
            }
            else {
                if (init == null)
                    throw new ArgumentException("First generation needs badges");

                AnimalBadges = init.Where(b => b.CalculateInherit())
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

        //Make breed method
        public Egg Breed(Animal partner,PetOwner owner,string name) {
            if (partner.Gender == Gender)
                throw new ArgumentException("You need to have two different genders");

            if (partner.Type != Type)
                throw new ArgumentException("You can only breed two animals of the same type");

            if (!CanBreed)
                throw new ArgumentException("This animal can't breed");

            var female = (partner.Gender == AnimalGender.Female ? partner : this);
            var male = (female.Equals(partner) ? this : partner);

            female.Pregnant = true;

            var egg = new Egg(female, male,name,owner);
            return egg;
        }

        public void IncreaseHungerAndBoredom() {
            if (Hunger >= 100 && Boredom >= 100) {
                StarveChanceRoll();
            }else {

                if (Hunger >= 100) {
                    StarveChanceRoll();
                    Hunger = 100;
                } else
                    Hunger += 1;

                if (Boredom >= 100) {
                    RunChanceRoll();
                    Boredom = 100;
                }
                else
                    Boredom += 2;
            }
        }

        private void StarveChanceRoll() {
            int r = rand.Next(0, 1001);
            IsDeceased = (r <= (StarveChance * 10));
        }

        private void RunChanceRoll() {
            int r = rand.Next(0, 1001);
            RanAway = (r <= (RunAwayChance * 10));
        }
        #endregion
    }
}
