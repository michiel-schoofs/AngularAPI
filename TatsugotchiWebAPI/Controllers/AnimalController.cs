#region Imports
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
    [Route("api/PetOwner/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnimalsController:ControllerBase
    {
        #region Fields
            private readonly IPetOwnerRepository _poc;
            private readonly IAnimalRepository _animalRepo;
            private readonly IBadgeRepository _badgeRepo;
        #endregion

        #region Constructor
            /// <summary>
            /// Constructor takes three parameters and is handled automatically by ASP.NET
            /// </summary>
            /// <param name="poc">Constructor injection done by Services Providers</param>
            /// <param name="animalRepo">Constructor injection done by Services Providers</param>
            /// <param name="badgeRepository">Constructor injection done by Services Providers</param>
            public AnimalsController(IPetOwnerRepository poc, IAnimalRepository animalRepo,
                IBadgeRepository badgeRepository)
            {
                _poc = poc;
                _animalRepo = animalRepo;
                _badgeRepo = badgeRepository;
            }
        #endregion

        #region Api Functions
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
            [HttpPost("CreateInitialAnimal/{name}")]
            public ActionResult<Animal> CreateInitialAnimal(string name){
                PetOwner own = GetUser();

                if (own.Animals.Count() == 0 && !string.IsNullOrEmpty(name)){

                    var initBadges = _badgeRepo.GiveInitialBadges();
                    Animal an = new Animal(name, initBadges, own);

                    own.Animals.Add(an);
                    own.FavoriteAnimal = an;
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
            /// Set the favorite animal of the user that's logged in,
            /// This is the animal that's displayed in the angular application
            /// </summary>
            /// <param name="id">The id of the animal you want to set as a favorite</param>
            /// <returns>
            /// Returns the newly favorite animal
            /// <seealso cref="AnimalsController.GetFavoriteAnimal"/>
            /// </returns>
            [HttpPut("SetFavorite/{id}")]
            public ActionResult SetFavoriteAnimal(int id){
                PetOwner own = GetUser();
                Animal animal = _animalRepo.GetAnimal(id);

                if (animal == null)
                    ModelState.AddModelError("Error", "The animal you specified couldn't be found");
                else
                {
                    if (animal.Owner == own)
                    {
                        own.FavoriteAnimal = animal;
                        _animalRepo.SaveChanges();
                        return CreatedAtAction(nameof(GetFavoriteAnimal), 
                                                new AnimalDTO(own.FavoriteAnimal));
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "You aren't the owner of this animal");
                    }
                }

                return BadRequest(ModelState);
            } 
            
            /// <summary>
            /// Gets the favorite animal from the user
            /// </summary>
            /// <returns>
            /// The favorite animal or an error if the user doesn't have a favorite animal
            /// </returns>
            [HttpGet("GetFavorite")]
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
            /// Get the animal with a specified ID only works if the animal is from the user.
            /// </summary>
            /// <param name="id">The id of the animal you want to view</param>
            /// <returns>Return a DTO of the animal</returns>
            [HttpGet("GetAnimal/{id}")]
            public ActionResult<AnimalDTO> GetAnimal(int id) {
                PetOwner own = GetUser();
                Animal an = _animalRepo.GetAnimal(id);

                if (an != null && an.Owner == own) {
                    return new AnimalDTO(an);
                }

                if (an == null) 
                    ModelState.AddModelError("Error Animal", "The animal is not found");

                if (an?.Owner != own) 
                    ModelState.AddModelError("Error Owner", "You aren't the owner of this animal");

                return BadRequest(ModelState);
            }

            /// <summary>
            /// Return all the animals owned by the current user
            /// </summary>
            /// <returns>All the animals owned by the user that's logged in</returns>
            [HttpGet("GetAllOwnedAnimals")]
            public ActionResult<List<AnimalDTO>> GetAllOwnedAnimals(){
                PetOwner owner = GetUser();
                ICollection<Animal> animals = owner.Animals;
                return animals.Select(a => new AnimalDTO(a))
                              .OrderBy(ad=>ad.ID).ToList();
            }

        #endregion

        #region Private Helper Functions
            private PetOwner GetUser(){
                return _poc.GetByEmail(User.Identity.Name);
            } 
        #endregion
    }
}
