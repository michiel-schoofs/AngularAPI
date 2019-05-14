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
                new Animal("Emma",initBadges),
                new Animal("Jan", initBadges),
                new Animal("Glenn",initBadges),
                new Animal("Michiel",initBadges),
                new Animal("Rudolf",initBadges),
                new Animal("Sien",initBadges),
                new Animal("Shana",initBadges),
                new Animal("Ria",initBadges),
                new Animal("Renée",initBadges)
            };

            _context.Animals.AddRange(animals);
            _context.SaveChanges();
        }
    }
}
