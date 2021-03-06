﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using TatsugotchiWebAPI.DTO;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Enums;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Data.Repository {
    public class DataInitializer {
        private ApplicationDBContext _context { get; set; }
        private UserManager<IdentityUser> _um;
        private ICollection<PetOwner> _petOwners;
        private Item[] items;

        private static readonly string[] names = new string[]{"Kesha","Morton","Glen","Alease","Ermelinda","Aracelis"
            ,"Clara","Francesco","Trula","Sonya","Maryland","Minerva","Blanch","Jaimee","Wilton","Salena",
            "Russel","Josphine","Aimee","Kate","Dominique","Carolyne","Tyrone","Vertie","Natosha",
            "Sherril","Stanford","Ettie","Estelle","Teofila"};
        private static readonly Random rand = new Random();

        public DataInitializer(ApplicationDBContext context ,UserManager<IdentityUser> um) {
            _context = context;
            _petOwners = new List<PetOwner>();
            _um = um;
        }

        private string GetRandomName(){
            int r = rand.Next(0, names.Length);
            return names[r];
        } 

        public void Seed() {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            AddUsers();
            AddBadges();
            AddAnimals();
            AddListings();
            AddSpecialCases();
            SetUpBreedingBug();
            AddItems();
            SeedMarket();
        }



        private void SeedMarket(){
            var market = Market.GetMarket();

            foreach (Item it in items) {
                market.AddListing(new MarketListing(it, 100));
            }

            _context.Market.Add(market);
            _context.SaveChanges();
        }

        private void AddUsers(){
            //web4 gelukkiggeennetbeans
            RegisterDTO dto = new RegisterDTO()
            {
                Email = "web4@hogent.be",
                BirthDay = DateTime.Now.AddYears(-25),
                Password = "gelukkiggeennetbeans",
                Username = "web4"
            };

            PetOwner po = new PetOwner(dto);
            _context.PetOwners.Add(po);
            _um.CreateAsync(new IdentityUser() { UserName = dto.Email, Email = dto.Email }, dto.Password).Wait();
            _petOwners.Add(po);

            dto = new RegisterDTO(){
                Email = "test@mail.be",
                BirthDay = DateTime.Now.AddYears(-22),
                Password = "string12345",
                Username = "test"
            };

            po = new PetOwner(dto);
            _context.PetOwners.Add(po);
            _um.CreateAsync(new IdentityUser() { UserName = dto.Email, Email = dto.Email },dto.Password).Wait();
            _petOwners.Add(po);

            dto = new RegisterDTO() {
                Email = "listingAccout@test.be",
                BirthDay = DateTime.Now.AddYears(-25),
                Password = "string12345",
                Username = "lister"
            };

            po = new PetOwner(dto);
            _context.PetOwners.Add(po);
            _um.CreateAsync(new IdentityUser() { UserName = dto.Email, Email = dto.Email }, dto.Password).Wait();
            _petOwners.Add(po);


            _context.SaveChanges();
        }

        public void AddBadges() {
            var badges = new Badge[] {

                //Negative Badges
                new Badge("Ieuwww stinky","smoker",0.75,BadgeType.Negative,true),
                new Badge("Hard breakup :c","Depressed",0.25,BadgeType.Negative),
                new Badge("*Gulp gulp* that's some good poison","Drinker",0.4,BadgeType.Negative,true),
                new Badge("GIVE ME MOREEEEEEEE","Greedy",0.87,BadgeType.Negative,true),
                new Badge("Screw THESE RULES","Anarchist",0.1,BadgeType.Negative),
                //Cute Badges
                new Badge("Sparkles all around","Sparkling Beauty",0.75,BadgeType.Cute,true),
                new Badge("KAWAAIIIIIIII","Anime lover",0.25,BadgeType.Cute),
                new Badge("*HATCHIEEEE*","Cute sneezing",0.4,BadgeType.Cute,true),
                new Badge("Wanna grab a drink?","Flirtatious",0.87,BadgeType.Cute,true),
                new Badge("Goddess","Godlike",0.1,BadgeType.Cute),
                //Cool badges
                new Badge("Puts in cereal before the milk","Normie power",0.75,BadgeType.Cool,true),
                new Badge("Goes trough hell and back","Coragous",0.25,BadgeType.Cool),
                new Badge("True master of trades","Economic genius",0.4,BadgeType.Cool,true),
                new Badge("Sunglass pro","Sunny",0.87,BadgeType.Cool,true),
                new Badge("Skating and rad kid with torn jeans","Skater boi",0.1,BadgeType.Cool),
                //Neutral Bages
                new Badge("Dying on the inside","Anxious",0.75,BadgeType.Neutral,true),
                new Badge("Come fly with me, lets fly, fly awayyyy","Traveller",0.25,BadgeType.Neutral),
                new Badge("WAKE ME UP INSIDEEE, Can't wake up","Evanescense fan",0.4,BadgeType.Neutral,true),
                //Diverse
                new Badge("Bad boku no pico noo","Penguin",0.05,BadgeType.Negative,true),
                new Badge("A bit of Birmingham delight","Druggie",0.87,BadgeType.Neutral),
                new Badge("He is better then the rest...","Pretentious",0.75,BadgeType.Negative),
                new Badge("Naughty little fellow","Sexual deviant",0.05,BadgeType.Positive),
                new Badge("It's not a phase mom","Emo",0.25,BadgeType.Positive,true),
                new Badge("Being the drivingforce behind progress","Worker",0.4,BadgeType.Positive) ,
                new Badge("Today I don't feel like doing anything","Lazy",0.87,BadgeType.Positive),
                new Badge("I wasn't cut out for this world","Doomer",0.5,BadgeType.Negative,true),
                new Badge("HONK HONK","Honker",0.4,BadgeType.Neutral,true),
                new Badge("The world is meaningless that's why I made my own","Bloomer",0.25,BadgeType.Positive,true),
                new Badge("Wake me up before you go-go","Boomer",0.3,BadgeType.Cool,true),
                new Badge("Slays the ladies, eats the pussies","Chad",0.02,BadgeType.Positive,true),
                new Badge("I swear it's just my bone structure","Incel",0.04,BadgeType.Negative,true)
            };

            _context.Badges.AddRange(badges);
            _context.SaveChanges();
        }

        public void AddAnimals()
        {
            var initBadges = _context.Badges.Where(b => b.IsInit).ToList();
            var web4User = _petOwners.FirstOrDefault(po => po.Username.Equals("web4"));
            var testUser = _petOwners.FirstOrDefault(po => po.Username.Equals("test"));

            var animals = new Animal[] {
                new Animal("Emma",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-10),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,web4User),
                new Animal("Jan",AnimalType.Capybara,AnimalGender.Male,DateTime.Now.AddDays(-10),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,testUser),
                new Animal("Glenn",AnimalType.Capybara,AnimalGender.Male,DateTime.Now.AddDays(-25),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,web4User),
                new Animal("Rudolf",AnimalType.Capybara,AnimalGender.Male,DateTime.Now.AddDays(-28),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,web4User),
                new Animal("Sia",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-31),
                initBadges,true,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,testUser),
                new Animal("Ria",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-2),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,testUser),
                new Animal("Renée",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-20),
                initBadges,false,false,true,GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,GetRandomNumber(),testUser),
                new Animal("Tiana",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-40),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,web4User),
                new Animal("Olivia",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-26),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,testUser),
                new Animal("Mia",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-17),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,web4User),
                new Animal("Charlotte",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-33),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,testUser)               
            };

            _context.Animals.AddRange(animals);
            _context.SaveChanges();

            Breed(animals);
        }

        private void AddListings() {
            var lister = _petOwners.FirstOrDefault(po => po.Username.Equals("lister"));
            var initBadges = _context.Badges.ToList();

            Animal[] animals = {
                 new Animal("Deborah",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-15),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,lister),
                new Animal("Neve",AnimalType.Capybara,AnimalGender.Male,DateTime.Now.AddDays(-8),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,lister),
                new Animal("Nina",AnimalType.Capybara,AnimalGender.Female,DateTime.Now.AddDays(-12),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,lister),
                new Animal("George",AnimalType.Capybara,AnimalGender.Male,DateTime.Now.AddDays(-2),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,lister),
                new Animal("Svitlana",AnimalType.Tapir,AnimalGender.Female,DateTime.Now.AddDays(-10),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,lister),
                new Animal("Louise",AnimalType.Tapir,AnimalGender.Male,DateTime.Now.AddDays(-7),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,lister),
                new Animal("Bert",AnimalType.Tapir,AnimalGender.Male,DateTime.Now.AddDays(-16),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,lister),
                new Animal("Sara",AnimalType.Alpaca,AnimalGender.Female,DateTime.Now.AddDays(-4),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,lister),
                new Animal("Daniel",AnimalType.Alpaca,AnimalGender.Male,DateTime.Now.AddDays(-9),
                initBadges,false,false,false,GetRandomNumber(),GetRandomNumber(),GetRandomNumber(),GetRandomNumber()
                ,lister)
            };

            _context.Animals.AddRange(animals);
            _context.SaveChanges();

            foreach (var animal in animals) {
                Listing listing = new Listing(animal, true, false);
                _context.Listings.Add(listing);
            }

            _context.SaveChanges();
        }

        private int GetRandomNumber(){

            return rand.Next(1, 101);
        }

        private void AddItems() {
            var testUser = _petOwners.FirstOrDefault(po => po.Username.Equals("test"));

            var pizzaHawai = new Item(20, 40, ItemCategory.Food, "sdfsqdf", "Pizza Hawaii");
            Item ninSwitch = new Item(60, 25, ItemCategory.Entertainment, "sdfsqdf", "Nintendo Switch");
            Item lasgna = new Item(29, 30, ItemCategory.Food, "sdfsqdf", "Lasagna");
            var tuna = new Item(80, 100, ItemCategory.Food, "sdfsqdf", "Canned tuna");

            items = new Item[] {
                new Item(100,10,ItemCategory.Entertainment,"sdfsqdf","TV"),
                pizzaHawai,
                ninSwitch,
                lasgna,
                tuna
            };

            _context.Items.AddRange(items);

            testUser.AddItem(pizzaHawai);
            testUser.AddItem(ninSwitch, 10);

            testUser.AddItem(tuna, 15);
            testUser.RemoveItem(tuna);
            testUser.RemoveItem(tuna,4);

            testUser.AddItem(lasgna, 10);
            testUser.RemoveItem(lasgna,10);

            _context.SaveChanges();
        }

        public void Breed(Animal[] animals) {
            Random rand = new Random();
            List<Egg> eggs = new List<Egg>();

            var testUser = _petOwners.FirstOrDefault(po => po.Username.Equals("test"));
            var web4User = _petOwners.FirstOrDefault(po => po.Username.Equals("web4"));

            var Males = animals.Where(a => a.Gender == AnimalGender.Male && a.CanBreed).ToList();
            var Females = animals.Where(a => a.Gender == AnimalGender.Female && a.CanBreed).ToList();

            foreach(var fa in Females) {
                var r = rand.Next(0, Males.Count - 1);
                var x = rand.Next(0, 2);

                if (x == 1) {
                    eggs.Add(fa.Breed(Males[r],GetRandomName()));
                }else {
                    eggs.Add(Males[r].Breed(fa,GetRandomName()));
                }
            }

            _context.Eggs.AddRange(eggs);
            _context.SaveChanges();
        }

        private async void SetUpBreedingBug(){
            var testUser = _petOwners.FirstOrDefault(p => p.Username.Equals("test"));

            var dto = new RegisterDTO()
            {
                Email = "bugUser@mail.be",
                BirthDay = DateTime.Now.AddYears(-24),
                Password = "string12345",
                Username = "test"
            };

            PetOwner po = new PetOwner(dto);
            _context.PetOwners.Add(po);
            _um.CreateAsync(new IdentityUser() { UserName = dto.Email, Email = dto.Email }, dto.Password).Wait();
            _petOwners.Add(po);

            var initBadges = _context.Badges.Where(b => b.IsInit).ToList();

            Animal poAF= new Animal("Tiana", AnimalType.Capybara, AnimalGender.Female, DateTime.Now.AddDays(-40),
               initBadges, false, false, false, GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber()
               , po);
            Animal poAM = new Animal("Jan", AnimalType.Capybara, AnimalGender.Male, DateTime.Now.AddDays(-10),
                initBadges, false, false, false, GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber()
                , po);

            Animal tuAF = new Animal("Test", AnimalType.Capybara, AnimalGender.Female, DateTime.Now.AddDays(-40),
               initBadges, false, false, false, GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber()
               , testUser);
            Animal tuAM = new Animal("Jan", AnimalType.Capybara, AnimalGender.Male, DateTime.Now.AddDays(-10),
                initBadges, false, false, false, GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), GetRandomNumber()
                , testUser);

            var egg1 = poAF.Breed(tuAM,"sdljfsdf");
            var egg2 = poAM.Breed(tuAF, "sfdmljk");

            _context.Animals.AddRange(new Animal[] { poAF, poAM, tuAF, tuAM });
            _context.Eggs.AddRange(new Egg[] { egg1, egg2 });

            _context.SaveChanges();

            var eggs = _context.Eggs.Where(e => e.Owner == po).ToList();
            foreach (var egg in eggs){
                _context.Eggs.Remove(egg);
            }

            _context.PetOwners.Remove(po);

            _context.SaveChanges();

            Animal an = egg2.Hatch();
            _context.Animals.Add(an);
            _context.Eggs.Remove(egg2);
            _context.SaveChanges();
        }

        private void AddSpecialCases() { 
            var initBadges = _context.Badges.Where(b => b.IsInit).ToList();
            var testUser = _petOwners.FirstOrDefault(po => po.Username.Equals("test"));
            var web4User = _petOwners.FirstOrDefault(po => po.Username.Equals("web4"));

            Animal male = new Animal("Bob", AnimalType.Capybara, AnimalGender.Male, DateTime.Now.AddDays(-20),
                initBadges, false, false, false, GetRandomNumber(), GetRandomNumber(), GetRandomNumber(), 
                GetRandomNumber(), testUser);

            Animal female = new Animal("Shana", AnimalType.Capybara, AnimalGender.Female, DateTime.Now.AddDays(-25),
                initBadges, false, false, false, GetRandomNumber(), GetRandomNumber(),
                GetRandomNumber(), GetRandomNumber(), testUser);

            var egg = female.Breed(male,GetRandomName());
            var an = egg.Hatch();
            
            var egg2 = male.Breed(female, GetRandomName());
            var an2 = egg2.Hatch();

            _context.Animals.AddRange(male, female, an,an2);

            _context.SaveChanges();
        }
    }
}
