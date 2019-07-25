﻿#region Imports
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
    [Route("api/PetOwners/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ListingsController:ControllerBase{
        #region Fields
            private readonly IPetOwnerRepository _poRepo;
            private readonly IAnimalRepository _aRepo; 
        #endregion

        #region Constructor
        public ListingsController(IPetOwnerRepository po, IAnimalRepository animalRepo)
            {
                _poRepo = po;
                _aRepo = animalRepo;
            } 
        #endregion

        #region Api methods
        /// <summary>
        /// Get the listings of the user that's currently logged in
        /// </summary>
        /// <returns>The list of Listings made by the logged in user.</returns>
        [HttpGet]
            public ActionResult<List<ListingDTO>> GetListings()
            {
                var user = GetOwner();
                ICollection<Listing> listings = user.Listings;
                return listings.Select(l => new ListingDTO(l)).ToList();
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

                try
                {
                    Animal an = _aRepo.GetAnimal(ldto.AnimalID);

                    if (an == null)
                        throw new Exception("The specified animal could not be found");

                    if (an.Owner != owner)
                        throw new InvalidListingException("You can't put up someone elses animal for sale or breeding.");

                    if (owner.AnimalInListing(an))
                        throw new InvalidListingException("This animal is already in a listing");

                    if (ldto.BreedAmount == 0 && ldto.AdoptAmount == 0)
                        owner.MakeListing(an, ldto.IsAdoptable, ldto.IsBreedable);
                    else
                        owner.MakeListing(an, ldto.IsAdoptable, ldto.IsBreedable
                            , ldto.AdoptAmount, ldto.BreedAmount);

                    _poRepo.SaveChanges();
                    //Gonna be replaced with a path later on to the get function for that listing
                    return ldto;

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Error", e.Message);
                }

                return BadRequest(ModelState);
            } 
        #endregion

        #region Helper functions
        private PetOwner GetOwner(){
                return _poRepo.GetByEmail(User.Identity.Name);
            } 
        #endregion
    }
}