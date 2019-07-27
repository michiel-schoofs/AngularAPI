using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TatsugotchiWebAPI.DTO;
using System.Linq;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;
using System;

namespace TatsugotchiWebAPI.Controllers
{
    /// <summary>
    /// Everything to do with eggs
    /// </summary>
    [ApiController]
    [Route("api/PetOwner/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EggsController : ControllerBase {
        private readonly IPetOwnerRepository _poRepo;
        private readonly IEggRepository _eggRepo;

        public EggsController(IPetOwnerRepository poRepo,IEggRepository eggRepo){
            _poRepo = poRepo;
            _eggRepo = eggRepo;
        }

        /// <summary>
        /// Return the eggs owned by the current user
        /// </summary>
        /// <returns>The eggs owned by the user loged in otherwise return null</returns>
        [HttpGet]
        public ActionResult<List<EggDTO>> GetEggsByUser(){
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
        public ActionResult<EggDTO> GetEggWithID(int id){
            try
            {
                var egg = _eggRepo.GetEggWithID(id);

                if (egg == null)
                    throw new Exception("We couldn't find the egg you're looking for");

                if (egg.Owner != GetOwner())
                    throw new Exception("This egg doesn't belong to the user that's logged in");

                return new EggDTO(egg);
            }
            catch (Exception e) {
                ModelState.AddModelError("Error", e.Message);
                return BadRequest(ModelState);
            }
        }

        private PetOwner GetOwner(){
            return _poRepo.GetByEmail(User.Identity.Name);
        }
    }
}
