using System;
using System.Collections.Generic;
using System.Linq;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.Data.Repository {
    public class DataInitializer {
        private ApplicationDBContext _context { get; set; }

        public DataInitializer(ApplicationDBContext context) {
            _context = context;
        }

        public void Seed() {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            AddBadges();
            AddAnimals();
        }

        public void AddBadges() {
            var badges = new Badge[] {
                //Negative Badges
                new Badge("Ieuwww stinky","smoker",0.75,BadgeType.negative,true),
                new Badge("Hard breakup :c","Depressed",0.25,BadgeType.negative),
                new Badge("*Gulp gulp* that's some good poison","Drinker",0.4,BadgeType.negative,true),
                new Badge("GIVE ME MOREEEEEEEE","Greedy",0.87,BadgeType.negative,true),
                new Badge("Screw THESE RULES","Anarchist",0.1,BadgeType.negative),
                //Cute Badges
                new Badge("Sparkles all around","Sparkling Beauty",0.75,BadgeType.cute,true),
                new Badge("KAWAAIIIIIIII","Anime lover",0.25,BadgeType.cute),
                new Badge("*HATCHIEEEE*","Cute sneezing",0.4,BadgeType.cute,true),
                new Badge("Wanna grab a drink?","Flirtatious",0.87,BadgeType.cute,true),
                new Badge("Goddess","Godlike",0.1,BadgeType.cute),
                //Cool badges
                new Badge("Puts in cereal before the milk","Normie power",0.75,BadgeType.cool,true),
                new Badge("Goes trough hell and back","Coragous",0.25,BadgeType.cool),
                new Badge("True master of trades","Economic genius",0.4,BadgeType.cool,true),
                new Badge("Sunglass pro","Sunny",0.87,BadgeType.cool,true),
                new Badge("Skating and rad kid with torn jeans","Skater boi",0.1,BadgeType.cool),
                //Neutral Bages
                new Badge("Dying on the inside","Anxious",0.75,BadgeType.neutral,true),
                new Badge("Come fly with me, lets fly, fly awayyyy","Traveller",0.25,BadgeType.neutral),
                new Badge("WAKE ME UP INSIDEEE, Can't wake up","Evanescense fan",0.4,BadgeType.neutral,true),
                //Diverse
                new Badge("Bad boku no pico noo","Penguin",0.05,BadgeType.negative,true),
                new Badge("A bit of Birmingham delight","Druggie",0.87,BadgeType.neutral),
                new Badge("He is better then the rest...","Pretentious",0.75,BadgeType.negative),
                new Badge("Naughty little fellow","Sexual deviant",0.05,BadgeType.positive),
                new Badge("It's not a phase mom","Emo",0.25,BadgeType.positive,true),
                new Badge("Being the drivingforce behind progress","Worker",0.4,BadgeType.positive) ,
                new Badge("Today I don't feel like doing anything","Lazy",0.87,BadgeType.positive),
                new Badge("I wasn't cut out for this world","Doomer",0.5,BadgeType.negative,true),
                new Badge("HONK HONK","Honker",0.4,BadgeType.neutral,true),
                new Badge("The world is meaningless that's why I made my own","Bloomer",0.25,BadgeType.positive,true),
                new Badge("Wake me up before you go-go","Boomer",0.3,BadgeType.cool,true),
                new Badge("Slays the ladies, eats the pussies","Chad",0.02,BadgeType.positive,true),
                new Badge("I swear it's just my bone structure","Incel",0.04,BadgeType.negative,true)
            };

            _context.Badges.AddRange(badges);
            _context.SaveChanges();
        }

        public void AddAnimals() {
            var initBadges = _context.Badges.Where(b => b.IsInit).ToList();

            var animals = new Animal[] {
                new Animal("Tom",initBadges),
                new Animal("Emma",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-10),
                initBadges,false,false,false,0,0),
                new Animal("Jan",AnimalType.Capybara,AnimalGender.Male,DateTime.Now.AddDays(-10),
                initBadges,false,false,false,0,0),
                new Animal("Glenn",AnimalType.Capybara,AnimalGender.Male,DateTime.Now.AddDays(-25),
                initBadges,false,false,false,0,0),
                new Animal("Michiel",AnimalType.Capybara,AnimalGender.Male,DateTime.Now.AddDays(-17),
                initBadges,false,false,false,0,0),
                new Animal("Rudolf",AnimalType.Capybara,AnimalGender.Male,DateTime.Now.AddDays(-28),
                initBadges,false,false,false,0,0),
                new Animal("Sia",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-31),
                initBadges,true,false,false,0,0),
                new Animal("Shana",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-12),
                initBadges,false,true,false,0,0),
                new Animal("Ria",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-2),
                initBadges,false,false,false,0,100),
                new Animal("Renée",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-20),
                initBadges,false,false,true,100,0),
                new Animal("Tiana",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-40),
                initBadges,false,false,false,0,0),
                new Animal("Olivia",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-26),
                initBadges,false,false,false,0,0),
                new Animal("Mia",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-17),
                initBadges,false,false,false,0,0),
                new Animal("Charlotte",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-33),
                initBadges,false,false,false,0,0)
            };

            _context.Animals.AddRange(animals);
            _context.SaveChanges();

            Breed(animals);
        }

        public void Breed(Animal[] animals) {
            Random rand = new Random();
            List<Egg> eggs = new List<Egg>();

            var Males = animals.Where(a => a.Gender == AnimalGender.Male && a.CanBreed).ToList();
            var Females = animals.Where(a => a.Gender == AnimalGender.Female && a.CanBreed).ToList();

            foreach(var fa in Females) {
                var r = rand.Next(0, Males.Count - 1);
                var x = rand.Next(0, 2);

                if (x == 1) {
                    eggs.Add(fa.Breed(Males[r]));
                }else {
                    eggs.Add(Males[r].Breed(fa));
                }
            }

            _context.Eggs.AddRange(eggs);
            _context.SaveChanges();
        }
    }
}
