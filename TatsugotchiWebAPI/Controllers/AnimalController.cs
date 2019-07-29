#region Imports
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;
using System.Linq;
using TatsugotchiWebAPI.DTO;
using System.Collections.Generic; 
#endregion

namespace TatsugotchiWebAPI.Controllers
{
    /// <summary>
    /// Api Controller for everything that has to do with Animals.
    /// </summary>
    [Route("Api/PetOwner/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnimalsController:ControllerBase
    {
        #region Fields
            private readonly IPetOwnerRepository _poc;
            private readonly IAnimalRepository _animalRepo;
            private readonly IBadgeRepository _badgeRepo;
            private readonly IEggRepository _eggRepo;
            private readonly IItemRepository _itemRepo;
        #endregion

        #region Constructor
            /// <summary>
            /// Constructor takes three parameters and is handled automatically by ASP.NET
            /// </summary>
            /// <param name="poc">Constructor injection done by Services Providers</param>
            /// <param name="animalRepo">Constructor injection done by Services Providers</param>
            /// <param name="badgeRepository">Constructor injection done by Services Providers</param>
            /// <param name="eggRepo">Constructor injection done by Services Providers</param>
            public AnimalsController(IPetOwnerRepository poc, IAnimalRepository animalRepo,
                IBadgeRepository badgeRepository,IEggRepository eggRepo,IItemRepository itemRepo)
            {
                _poc = poc;
                _animalRepo = animalRepo;
                _badgeRepo = badgeRepository;
                _eggRepo = eggRepo;
                _itemRepo = itemRepo;
            }
        #endregion

        #region Api Functions
            /// <summary>
            /// Return all the animals owned by the current user
            /// </summary>
            /// <returns>All the animals owned by the user that's logged in</returns>
            [HttpGet]
            public ActionResult<List<AnimalDTO>> GetAllOwnedAnimals()
            {
                PetOwner owner = GetUser();
                ICollection<Animal> animals = owner.Animals;
                return animals.Select(a => new AnimalDTO(a))
                              .OrderBy(ad => ad.ID).ToList();
            }
            /// <summary>
            ///     Create an intial animal if you have no animals, otherwise returns an error
        ///     <seealso cref="AnimalsController.GetAnimal(int)"/>
        /// </summary>
        /// <param name="name">
        ///     The name of the animal which you are gonna generate
        /// </param>
        /// <returns>
        ///     Returns the path to the newly generated animal
        /// </returns>
        [HttpPost("Create/{name}")]
            public ActionResult<Animal> CreateInitialAnimal(string name){
                PetOwner own = GetUser();

                if (own.Animals.Count() == 0 && !string.IsNullOrEmpty(name)){

                    var initBadges = _badgeRepo.GiveInitialBadges();
                    Animal an = new Animal(name, initBadges, own);

                    own.Animals.Add(an);
                    an.IsFavorite = true;
                    _animalRepo.SaveChanges();

                    return CreatedAtAction(nameof(GetAnimal), new { id = an.ID }, new AnimalDTO(an));
                }

               if (own.Animals.Count() != 0)
                    ModelState.AddModelError("Error Animals", "You already have animals");

                if (string.IsNullOrEmpty(name))
                    ModelState.AddModelError("Error Name", "Please provide a name for your animal.");

                return BadRequest(ModelState);
            }

            /// <summary>
            /// Get the animal with a specified ID only works if the animal is from the user.
            /// </summary>
            /// <param name="id">The id of the animal you want to view</param>
            /// <returns>Return a DTO of the animal</returns>
            [HttpGet("{id}")]
            public ActionResult<AnimalDTO> GetAnimal(int id)
            {
                try
                {
                    Animal an = GetAnimalWithIDAndUser(id, GetUser());
                    return new AnimalDTO(an);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// Get the eggs where the animal specified by the id is the parent off
            /// </summary>
            /// <param name="id">The id of the animal you want to view the eggs off</param>
            /// <returns>The eggs that the specified animal has or if the id doesn't correspond to an animal
            /// Or an animal owned by the user return a bad request.
            /// </returns>
            [HttpGet("{id}/Eggs")]
            public ActionResult<ICollection<EggDTO>> GetEggsWithAnimal(int id)
            {
                try
                {
                    var user = GetUser();
                    var an = GetAnimalWithIDAndUser(id, user);

                    return _eggRepo.GetEggsFromAnimalOwnedByUser(an, user)
                                    .Select(e => new EggDTO(e)).ToList();

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// Deletes the specified animal
            /// </summary>
            /// <param name="id">The id of the animal you want to delete</param>
            /// <returns>If the request was succefull (animal is owned by the user and exists) returns an 
            /// ok and the DTO of the animal. Otherwise return badrequest.
            /// </returns>
            [HttpDelete("{id}/Delete")]
            public ActionResult<AnimalDTO> DeleteAnimal(int id)
            {
                try
                {
                    Animal an = GetAnimalWithIDAndUser(id, GetUser());
                    _animalRepo.RemoveAnimal(an);
                    _animalRepo.SaveChanges();
                    return Ok(new AnimalDTO(an));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }
            
            /// <summary>
            /// Breeds animals that are owned by the user
            /// </summary>
            /// <param name="id">The id of the animal you want to breed</param>
            /// <param name="idOtherAnimal">The id you want the aforementioned animal to breed with</param>
            /// <param name="name">The name of the animal that will hatch from the egg</param>
            /// <returns>Returns a dto of the egg as a result of the breeding process.
            /// If something went wrong return a bad request with the error.</returns>
            [HttpPost("{id}/Breed/{idOtherAnimal}/{name}")]
            public ActionResult<EggDTO> BreedAnimals(int id,int idOtherAnimal,string name){
                try{
                    PetOwner user = GetUser();

                    Animal animalOne = GetAnimalWithIDAndUser(id, user);
                    Animal animalTwo = GetAnimalWithIDAndUser(idOtherAnimal, user);
                    Egg egg = animalOne.Breed(animalTwo, name);

                    _eggRepo.AddEgg(egg);
                    _eggRepo.SaveChanges();

                    return Created($"Api/PetOwner/Eggs/{egg.ID}", new EggDTO(egg));
                 }catch (Exception e) { 
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// Feeds the animal with the specified id, the specified item
            /// </summary>
            /// <param name="id">the id of the animal you want to feed</param>
            /// <param name="itemID">the id of the item you want to feed to the animal</param>
            /// <returns>If it was succesfull return the AnimalDTO else return bad request</returns>
            [HttpPut("{id}/Feed/{itemID}")]
            public ActionResult<AnimalDTO> Feed(int id, int itemID){
                try{
                    PetOwner user = GetUser();
                    Animal animal = GetAnimalWithIDAndUser(id, user);

                    Item item = GetItemWithId(itemID);
                    CheckItemInventory(user, item, 1);

                    animal.FeedAnimal(item);
                    user.RemoveItem(item);
                    _animalRepo.SaveChanges();
                    return Ok(new AnimalDTO(animal));
                }catch (Exception e) {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// entertain the animal with the specified id, the specified item
            /// </summary>
            /// <param name="id">the id of the animal you want to entertain</param>
            /// <param name="itemID">the id of the item you want to entertain to the animal</param>
            /// <returns>If it was succesfull return the AnimalDTO else return bad request</returns>
            [HttpPut("{id}/Entertain/{itemID}")]
            public ActionResult<AnimalDTO> Entertain(int id, int itemID)
            {
                try
                {
                    PetOwner user = GetUser();
                    Animal animal = GetAnimalWithIDAndUser(id, user);

                    Item item = GetItemWithId(itemID);
                    CheckItemInventory(user, item, 1);

                    animal.EntertainAnimal(item);
                    user.RemoveItem(item);
                    _animalRepo.SaveChanges();
                    return Ok(new AnimalDTO(animal));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

        /// <summary>
        /// Change the name of the animal.
        /// </summary>
        /// <param name="id">The id of the animal you want to change the name of</param>
        /// <param name="name">The new name of the animal</param>
        /// <returns>Changes the name of the animal and returns the animal if it's a valid request
        /// (the specified animal exists and is owned by the user that's logged in). Otherwise returns a bad request.
        /// </returns>
        [HttpPatch("{id}/Name/{name}")]
            public ActionResult<AnimalDTO> ChangeName(int id, string name)
            {
                try{
                    if (string.IsNullOrEmpty(name))
                        throw new Exception("this is an invalid name");

                    Animal an = GetAnimalWithIDAndUser(id, GetUser());
                    an.Name = name;
                    _animalRepo.SaveChanges();
                    return Ok(new AnimalDTO(an));
                }catch (Exception e) { 
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

        /// <summary>
        /// Gets the favorite animal from the user
        /// </summary>
        /// <returns>
        /// The favorite animal or an error if the user doesn't have a favorite animal
        /// </returns>
        [HttpGet("Favorite")]
            public ActionResult<AnimalDTO> GetFavoriteAnimal(){
                PetOwner own = GetUser();

                if (own.FavoriteAnimal == null) {
                    ModelState.AddModelError("Error", "This user doesn't have a favorite animal");
                    return BadRequest(ModelState);
                }

                Animal an = own.FavoriteAnimal;
                return new AnimalDTO(an);
            }

        /// <summary>
        /// Set the favorite animal of the user that's logged in,
        /// This is the animal that's displayed in the angular application
        /// </summary>
        /// <param name="id">The id of the animal you want to set as a favorite</param>
        /// <returns>
        /// Returns the newly favorite animal
        /// <seealso cref="AnimalsController.GetFavoriteAnimal"/>
        /// </returns>
        [HttpPut("Favorite/{id}")]
        public ActionResult SetFavoriteAnimal(int id)
        {
            try
            {
                PetOwner own = GetUser();
                Animal animal = GetAnimalWithIDAndUser(id, own);

                if (own.FavoriteAnimal != null)
                    own.FavoriteAnimal.IsFavorite = false;

                animal.IsFavorite = true;

                _animalRepo.SaveChanges();
                return CreatedAtAction(nameof(GetFavoriteAnimal),
                                    new AnimalDTO(own.FavoriteAnimal));

            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
                return BadRequest(ModelState);
            }
        }

        #endregion

        #region Private Helper Functions
            private PetOwner GetUser(){
                return _poc.GetByEmail(User.Identity.Name);
            }

            private Item GetItemWithId(int id){
                var item = _itemRepo.GetItem(id);

                if (item == null)
                    throw new Exception("This item doesn't exist");

                return item;
            }

            private void CheckItemInventory(PetOwner po, Item item,int quantity){
                if (!po.ContainsItem(item))
                    throw new Exception("You don't have that item in your inventory");

                if (po.GetQuantity(item) < quantity)
                    throw new Exception("you don't have enough of this item");
            }
        
            private Animal GetAnimalWithIDAndUser(int id,PetOwner user){
                var an = _animalRepo.GetAnimal(id);

                if (an == null)
                    throw new Exception("We couldn't find the animal you're looking for");

                if (an.Owner != user)
                    throw new Exception("You aren't the owner of this animal");

                return an;
            }
        #endregion
    }
}
