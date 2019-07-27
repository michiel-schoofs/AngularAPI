using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TatsugotchiWebAPI.Model.Enums;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Model {
    public class Egg {
        public int ID { get; set; }

        public string Name { get; set; }
        public List<AnimalEgg> AnimalEggs { get; set; }
        [NotMapped]
        public List<Animal> Parents { get => AnimalEggs.Select(ae =>ae.An).ToList(); }
        [NotMapped]
        public Animal Mother { get => Parents.FirstOrDefault(a => a.Gender == AnimalGender.Female); }
        [NotMapped]
        public Animal Father { get => Parents.FirstOrDefault(a => a.Gender == AnimalGender.Male); }
        
        public DateTime DateConceived { get; set; }
        public AnimalType Type { get; set; }

        public PetOwner Owner { get; set; }

        [NotMapped]
        public TimeSpan TimeRemaining { get => (DateConceived.Add(Span)).Subtract(DateTime.Now); }
        [NotMapped]
        public TimeSpan Span { get{
                TimeSpan var = new TimeSpan();

                switch (this.Type) {
                    case AnimalType.Alpaca:
                        var = new TimeSpan(5,0,0,0);
                        break;
                    case AnimalType.Capybara:
                        var = new TimeSpan(0, 0, 0, 30);
                        /*Actual value
                        var = new TimeSpan(2, 12, 0, 0);
                        */
                        break;
                    case AnimalType.Tapir:
                        var = new TimeSpan(5, 0, 0, 0);
                        break;
                }

                return var;
            }
        }

        public Egg(Animal mother,Animal father,string name,PetOwner owner) {
            if (mother == null || father == null)
                throw new ArgumentException("Neither of the parents can be null");

            AnimalEggs = new List<AnimalEgg>();

            AnimalEggs.Add(new AnimalEgg() { An = mother, Egg = this });
            AnimalEggs.Add(new AnimalEgg() { An = father, Egg = this });

            Owner = owner;
            Name = name;

            DateConceived = DateTime.Now;
            Type = Mother.Type;
        }

        //EF Constructor
        protected Egg() {}

        public Animal Hatch() {
            Mother.Pregnant = false;
            return new Animal(Name, Type, new List<Animal>() { Mother,Father},Owner);
        }

    }
}
