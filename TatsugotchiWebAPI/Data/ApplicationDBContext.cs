using Microsoft.EntityFrameworkCore;
using TatsugotchiWebAPI.Data.Mapping;
using TatsugotchiWebAPI.Model;
using System.Linq;
using TatsugotchiWebAPI.Model.EFClasses;

namespace TatsugotchiWebAPI.Data {
    public class ApplicationDBContext : DbContext {

        public DbSet<Animal> Animals { get; }
        public DbSet<Badge> Badges { get; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<Animal>(new AnimalMapper())
                .ApplyConfiguration<ChildParentAnimal>(new ChildParenAnimalMapper())
                .ApplyConfiguration<AnimalBadges>(new AnimalBadgesMapper())
                .ApplyConfiguration<Badge>(new BadgesMapper());

            SeedData(modelBuilder);
        }

        public void SeedData(ModelBuilder modelBuilder) {
            var badges = new Badge[] {
                //Negative Badges
                new Badge("Ieuwww stinky","smoker",0.75,BadgeType.negative,true){ID=1},
                new Badge("Hard breakup :c","Depressed",0.25,BadgeType.negative){ ID=2},
                new Badge("*Gulp gulp* that's some good poison","Drinker",0.4,BadgeType.negative,true){ ID=3},
                new Badge("GIVE ME MOREEEEEEEE","Greedy",0.87,BadgeType.negative,true){ ID=4},
                new Badge("Screw THESE RULES","Anarchist",0.1,BadgeType.negative){ ID=5},
                //Cute Badges
                new Badge("Sparkles all around","Sparkling Beauty",0.75,BadgeType.cute,true){ ID=6},
                new Badge("KAWAAIIIIIIII","Anime lover",0.25,BadgeType.cute){ID=7},
                new Badge("*HATCHIEEEE*","Cute sneezing",0.4,BadgeType.cute,true){ ID=8},
                new Badge("Wanna grab a drink?","Flirtatious",0.87,BadgeType.cute,true){ID=9},
                new Badge("Goddess","Godlike",0.1,BadgeType.cute){ID=10},
                //Cool badges
                new Badge("Puts in cereal before the milk","Normie power",0.75,BadgeType.cool,true){ID=11},
                new Badge("Goes trough hell and back","Coragous",0.25,BadgeType.cool){ ID=12},
                new Badge("True master of trades","Economic genius",0.4,BadgeType.cool,true){ID=13},
                new Badge("Sunglass pro","Sunny",0.87,BadgeType.cool,true){ID=14},
                new Badge("Skating and rad kid with torn jeans","Skater boi",0.1,BadgeType.cool){ ID=15},
                //Neutral Bages
                new Badge("Dying on the inside","Anxious",0.75,BadgeType.neutral,true){ ID=16},
                new Badge("Come fly with me, lets fly, fly awayyyy","Traveller",0.25,BadgeType.neutral){ID=17},
                new Badge("WAKE ME UP INSIDEEE, Can't wake up","Evanescense fan",0.4,BadgeType.neutral,true){ID=18},
                //Diverse
                new Badge("Bad boku no pico noo","Penguin",0.05,BadgeType.negative,true){ID=19},
                new Badge("A bit of Birmingham delight","Druggie",0.87,BadgeType.neutral){ ID=20},
                new Badge("He is better then the rest...","Pretentious",0.75,BadgeType.negative){ ID=21},
                new Badge("Naughty little fellow","Sexual deviant",0.05,BadgeType.positive){ ID=22},
                new Badge("It's not a phase mom","Emo",0.25,BadgeType.positive,true){ID=23},
                new Badge("Being the drivingforce behind progress","Worker",0.4,BadgeType.positive) { ID=24},
                new Badge("Today I don't feel like doing anything","Lazy",0.87,BadgeType.positive){ ID=25}
            };

            modelBuilder.Entity<Badge>().HasData(badges);
            var initBadges = badges.Where(b => b.IsInit).ToList();


        }
    }
}
