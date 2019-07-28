#region Imports
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using TatsugotchiWebAPI.DTO;
    using System.Linq;
    using TatsugotchiWebAPI.Model;
    using TatsugotchiWebAPI.Model.Interfaces;
    using System; 
#endregion

namespace TatsugotchiWebAPI.Controllers
{
    /// <summary>
    /// Everything to do with eggs
    /// </summary>
    [ApiController]
    [Route("Api/PetOwner/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EggsController : ControllerBase {
        #region Fields
            private readonly IPetOwnerRepository _poRepo;
            private readonly IEggRepository _eggRepo; 
        #endregion

        #region Constructor
        public EggsController(IPetOwnerRepository poRepo, IEggRepository eggRepo)
            {
                _poRepo = poRepo;
                _eggRepo = eggRepo;
            } 
        #endregion

        #region API Methods
            /// <summary>
        /// Return the eggs owned by the current user
        /// </summary>
        /// <returns>The eggs owned by the user loged in otherwise return null</returns>
            [HttpGet]
            public ActionResult<List<EggDTO>> GetEggsByUser()
            {
                var eggs = _eggRepo.GetEggsByPetOwner(GetOwner());
                return eggs.Select(e => new EggDTO(e)).ToList();
            }

            /// <summary>
            /// Gets the egg with a specific ID, the egg needs to be owned by the user
            /// </summary>
            /// <param name="id">The ID of the egg you're trying to view</param>
            /// <returns>The DTO corresponding to this egg or 
            /// an badrequest of this egg doesn't belong to the loged in user</returns>
            [HttpGet("{id}")]
            public ActionResult<EggDTO> GetEggWithID(int id)
            {
                try
                {
                    var egg = GetEgg(id);
                    return new EggDTO(egg);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// Deletes the egg with the specified id
            /// </summary>
            /// <param name="id">The id of the egg that needs to be deleted</param>
            /// <returns>
            /// Bad request if the egg id couldn't be found or the user is not the owner,
            /// Otherwise return OK + the egg just deleted
            /// </returns>
            [HttpDelete("{id}/Delete")]
            public ActionResult<EggDTO> DeleteEggWithID(int id)
            {
                try
                {
                    var egg = GetEgg(id);
                    _eggRepo.RemoveEgg(egg);
                    _eggRepo.SaveChanges();
                    return Ok(new EggDTO(egg));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }
        #endregion

        #region Private helper methods
            private Egg GetEgg(int id){
                var egg = _eggRepo.GetEggWithID(id);

                if (egg == null)
                    throw new Exception("We couldn't find the egg you're looking for");

                if (egg.Owner != GetOwner())
                    throw new Exception("This egg doesn't belong to the user that's logged in");

                return egg;
            }

            private PetOwner GetOwner(){
                return _poRepo.GetByEmail(User.Identity.Name);
            } 
        #endregion
    }
}
