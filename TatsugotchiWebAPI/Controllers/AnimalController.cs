using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;
using System.Linq;

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
            /// </summary>
            /// <param name="name">
            ///     The name of the animal which you are gonna generate
            /// </param>
            /// <returns>
            ///     Returns the path to the newly generated animal
            /// </returns>
            [HttpPost("CreateInitialAnimal/{name}")]
            public ActionResult<Animal> CreateInitialAnimal(string name)
            {
                PetOwner po = _poc.GetByEmail(User.Identity.Name);

                if (po.Animals.Count() == 0 && !string.IsNullOrEmpty(name))
                {

                    var initBadges = _badgeRepo.GiveInitialBadges();
                    Animal an = new Animal(name, initBadges, po);

                    po.Animals.Add(an);
                    po.FavoriteAnimal = an;
                    _animalRepo.SaveChanges();

                    //Needs to be changed later on to animal get methode 
                    return Ok();
                }

                if (po.Animals.Count() != 0)
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
            /// <returns>Action result indicating succes or failure</returns>
            [HttpPut("SetFavorite/{id}")]
            public ActionResult SetFavoriteAnimal(int id)
            {
                PetOwner po = _poc.GetByEmail(User.Identity.Name);
                Animal animal = _animalRepo.GetAnimal(id);

                if (animal == null)
                    ModelState.AddModelError("Error", "The animal you specified couldn't be found");
                else
                {
                    if (animal.Owner == po)
                    {
                        po.FavoriteAnimal = animal;
                        _animalRepo.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "You aren't the owner of this animal");
                    }
                }

                return BadRequest(ModelState);
            } 
        #endregion

    }
}
