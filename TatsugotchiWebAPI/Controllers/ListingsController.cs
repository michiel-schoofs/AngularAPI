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
        #endregion

        #region Constructor
            public ListingsController(IPetOwnerRepository po, IAnimalRepository animalRepo,
                                       IListingRepository listingRepo){
                _poRepo = po;
                _aRepo = animalRepo;
                _listingRepo = listingRepo;
            }
        #endregion

        #region Api methods
            /*/// <summary>
            /// Get the listings of the user that's currently logged in
            /// </summary>
            /// <returns>The list of Listings made by the logged in user.</returns>
            [HttpGet]
            public ActionResult<List<ListingDTO>> GetListings()
            {
                var user = GetOwner();
                ICollection<Listing> listings = user.Listings;
                return listings.Select(l => new ListingDTO(l)).ToList();
            }    */

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
