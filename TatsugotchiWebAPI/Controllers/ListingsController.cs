#region Imports
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TatsugotchiWebAPI.DTO;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Exceptions;
using TatsugotchiWebAPI.Model.Interfaces; 
#endregion

namespace TatsugotchiWebAPI.Controllers
{ 
    /// <summary>
    /// Api controller for Listings
    /// </summary>
    [Route("Api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ListingsController:ControllerBase{
        #region Fields
            private readonly IPetOwnerRepository _poRepo;
            private readonly IAnimalRepository _aRepo;
            private readonly IListingRepository _listingRepo;
            private readonly IEggRepository _eggRepo;
        #endregion

        #region Constructor
            public ListingsController(IPetOwnerRepository po, IAnimalRepository animalRepo,
                                       IListingRepository listingRepo, IEggRepository eggRepo){
                _poRepo = po;
                _aRepo = animalRepo;
                _listingRepo = listingRepo;
                _eggRepo = eggRepo;
            }
        #endregion

        #region Api methods
            /// <summary>
            /// Get the listings not by the current logged in user
            /// </summary>
            /// <returns>Return the data transfer object of those listings</returns>
            [HttpGet]
            public ActionResult<List<ListingDTO>> GetListingByOtherUsers() {
                var listing = _listingRepo.GetListingsNotByUser(GetOwner());
                return listing.Select(l => new ListingDTO(l)).ToList();
            }

            /// <summary>
            /// Give the listing with a specific ID
            /// </summary>
            /// <param name="id">The id of the listing you want to view</param>
            /// <returns>A dto representing the Listing with the ID you specified,
            /// or a bad request if the listing doesn't exist</returns>
            [HttpGet("{id}")]
            public ActionResult<ListingDTO> GetListingWithID(int id){
                try{
                    var listing = GetListing(id);
                    return new ListingDTO(listing);
                } catch (Exception e) {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }


            /// <summary>
            /// Breeds with the animal that's put up on the listing
            /// </summary>
            /// <param name="id">The id of the listing you want your animal to breed with</param>
            /// <param name="idOfAnimal">The id of the animal you'll breed with</param>
            /// <param name="name">The name of the animal that you'll hatch.</param>
            /// <returns></returns>
            [HttpPost("{id}/Breed/{idOfAnimal}/{name}")]
            public ActionResult<EggDTO> BreedAnimalFromListing(int id, int idOfAnimal, string name) {
                try{
                    var listing = GetListing(id);
                    var user = GetOwner();
                    var animal = _aRepo.GetAnimal(idOfAnimal);

                    if (animal.Owner != user)
                        throw new Exception("You aren't the owner of the animal you're trying to breed");

                    var egg = listing.AcceptBreeding(user, animal, name);
                    _eggRepo.AddEgg(egg);

                    _listingRepo.RemoveListing(listing);
                    _eggRepo.SaveChanges();

                    return Created($"Api/PetOwner/Eggs/{egg.ID}", new EggDTO(egg));
                } catch (Exception e) {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// Adopt the animal of the specified listing
            /// </summary>
            /// <param name="id">The id of the listing you want to adopt the animal for</param>
            /// <returns>The DTO of the animal you just adopted or
            /// bad request if the adoption didn't succeed</returns>
            [HttpPatch("{id}/Adopt")]
            public ActionResult<AnimalDTO> AdoptTroughListing(int id){
                try{
                    var listing = GetListing(id);
                    listing.AcceptAdoption(GetOwner());
                    _listingRepo.RemoveListing(listing);
                    _listingRepo.SaveChanges();
                    return Ok(new AnimalDTO(listing.Animal));
                } catch (Exception e) {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// Updates the breeding amount for the specified listing
            /// </summary>
            /// <param name="id">The id of the listing you want to change</param>
            /// <param name="amount">The new amount for breeding in the listing</param>
            /// <returns>The changed listing as a dto or if something went wrong a bad request</returns>
            [HttpPatch("{id}/BreedingAmount/{amount}")]
            public ActionResult<ListingDTO> ChangeBreedingAmount(int id,int amount){
                try{
                    var list = GetListing(id);

                    if (list.Owner != GetOwner())
                        throw new Exception("You aren't the owner of the listing you're trying to change");

                    list.ChangeBreedingAmount(amount);
                    _listingRepo.SaveChanges();
                    return Ok(new ListingDTO(list));
                } catch (Exception e) {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// Updates the adoption amount for the specified listing
            /// </summary>
            /// <param name="id">The id of the listing you want to change</param>
            /// <param name="amount">The new amount for adoption in the listing</param>
            /// <returns>The changed listing as a dto or if something went wrong a bad request</returns>
            [HttpPatch("{id}/AdoptionAmount/{amount}")]
            public ActionResult<ListingDTO> ChangeAdoptionAmount(int id, int amount)
            {
                try
                {
                    var list = GetListing(id);

                    if (list.Owner != GetOwner())
                        throw new Exception("You aren't the owner of the listing you're trying to change");

                    list.ChangeAdoptionAmount(amount);
                    _listingRepo.SaveChanges();
                    return Ok(new ListingDTO(list));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// Deletes the listing with the specified ID
            /// </summary>
            /// <param name="id">The id of the listing you want to delete</param>
            /// <returns>The deleted listing or badrequest
            /// if the listing doesn't exist or you aren't the owner</returns>
            [HttpDelete("{id}/Delete")]
            public ActionResult<ListingDTO> DeleteListingWithID(int id){
                try{
                    Listing listing = GetListing(id);

                    if (listing.Owner != GetOwner())
                        throw new Exception("You aren't the owner of this listing");

                    _listingRepo.RemoveListing(listing);
                    _listingRepo.SaveChanges();

                     return Ok(new ListingDTO(listing));
                } catch(Exception e){
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// Adds a listing made by the user who's logged in
            /// </summary>
            /// <param name="ldto">The DTO corresponding to the listing you wanna make</param>
            /// <returns>The listing that was just added and the route to it</returns>
            [HttpPost("Add")]
            public ActionResult<ListingDTO> AddListing(ListingDTO ldto)
            {
                var owner = GetOwner();

                try{
                    Animal an = _aRepo.GetAnimal(ldto.AnimalID);

                    if (an == null)
                        throw new Exception("The specified animal could not be found");

                    if (an.Owner != owner)
                        throw new InvalidListingException("You can't put up someone elses animal for sale or breeding.");

                    if (_listingRepo.AnimalIsInListing(an))
                        throw new InvalidListingException("This animal is already in a listing");

                    Listing listing;

                    if (ldto.BreedAmount == 0 && ldto.AdoptAmount == 0)
                        listing = new Listing(an, ldto.IsAdoptable, ldto.IsBreedable);
                    else
                        listing = new Listing(an, ldto.IsAdoptable, ldto.IsBreedable
                            , ldto.AdoptAmount, ldto.BreedAmount);

                    _listingRepo.AddListing(listing);
                    _listingRepo.SaveChanges();

                    var list = _listingRepo.GetListingWithID(listing.ID);
                    return CreatedAtAction(nameof(GetListingWithID), new { id = listing.ID }
                                            , new ListingDTO(list));
                }catch (Exception e){
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            } 
        #endregion

        #region Helper functions
            private PetOwner GetOwner(){
                return _poRepo.GetByEmail(User.Identity.Name);
            } 

            private Listing GetListing(int id){
                var listing = _listingRepo.GetListingWithID(id);

                if (listing == null)
                    throw new Exception("This listing doesn't exist");

                return listing;
            }
        #endregion
    }
}
