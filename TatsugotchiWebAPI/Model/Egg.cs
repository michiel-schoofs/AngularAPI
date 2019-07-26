using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TatsugotchiWebAPI.Model.Enums;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Model {
    public class Egg {
        private static readonly string[] names = new string[]{"Kesha","Morton","Glen","Alease","Ermelinda","Aracelis"
            ,"Clara","Francesco","Trula","Sonya","Maryland","Minerva","Blanch","Jaimee","Wilton","Salena",
            "Russel","Josphine","Aimee","Kate","Dominique","Carolyne","Tyrone","Vertie","Natosha",
            "Sherril","Stanford","Ettie","Estelle","Teofila"};
        private static readonly Random rand = new Random();

        public int ID { get; set; }

        public List<AnimalEgg> AnimalEggs { get; set; }
        [NotMapped]
        public List<Animal> Parents { get => AnimalEggs.Select(ae =>ae.An).ToList(); }
        [NotMapped]
        public Animal Mother { get => Parents.FirstOrDefault(a => a.Gender == AnimalGender.Female); }
        [NotMapped]
        public Animal Father { get => Parents.FirstOrDefault(a => a.Gender == AnimalGender.Male); }

        public DateTime DateConceived { get; set; }
        public AnimalType Type { get; set; }



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

        public Egg(Animal mother,Animal father) {
            if (mother == null || father == null)
                throw new ArgumentException("Neither of the parents can be null");

            AnimalEggs = new List<AnimalEgg>();

            AnimalEggs.Add(new AnimalEgg() { An = mother, Egg = this });
            AnimalEggs.Add(new AnimalEgg() { An = father, Egg = this });

            DateConceived = DateTime.Now;
            Type = Mother.Type;
        }

        //EF Constructor
        protected Egg() {}

        public Animal Hatch() {
            int r = rand.Next(0, names.Count());
            Mother.Pregnant = false;
            return new Animal(names[r], Type, new List<Animal>() { Mother,Father});
        }

    }
}
