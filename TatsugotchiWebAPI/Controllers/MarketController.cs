#region Imports
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TatsugotchiWebAPI.DTO;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces; 
#endregion

namespace TatsugotchiWebAPI.Controllers
{
    [Route("Api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MarketController:ControllerBase{
        #region fields
            private readonly IPetOwnerRepository _poc;
            private readonly IMarketRepository _marketRepo;
        #endregion

        #region Constructor
        public MarketController(IPetOwnerRepository poc, IMarketRepository marktRepo)
        {
            _poc = poc;
            _marketRepo = marktRepo;
        }
        #endregion

        #region Api Methods
        /// <summary>
        /// Get all the listings of the market
        /// </summary>
        /// <returns>The listings of the market</returns>
        [HttpGet("Listings")]
        public ActionResult<List<MarketListingDTO>> GetAllTheListings()
        {
            Market market = GetMarket();
            return market.Listings.Select(ml => new MarketListingDTO(ml)).ToList();
        }

        /// <summary>
        /// Get the specified listing of the market
        /// </summary>
        /// <param name="id">The id of the listing you want to view</param>
        /// <returns>The dto of the listing</returns>
        [HttpGet("Listings/{id}")]
        public ActionResult<MarketListingDTO> GetListingWithId(int id)
        {
            try
            {
                Market market = GetMarket();
                return new MarketListingDTO(GetListingWithID(market, id));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Buy the item of the market
        /// </summary>
        /// <param name="id">The id of the market listing</param>
        /// <param name="amount">The amount that you want to buy</param>
        /// <returns>The inventory of the user</returns>
        [HttpPut("Listings/{id}/Buy/{amount}")]
        public ActionResult<List<POItemDTO>> BuyItem(int id, int amount)
        {
            try
            {
                PetOwner user = GetUser();

                Market markt = GetMarket();
                MarketListing ml = GetListingWithID(markt, id);

                int valueOfTransaction = ml.ListingAmount * amount;

                if (valueOfTransaction > user.WalletAmount)
                    throw new Exception("You don't have enough funds for the transaction");

                Item it = ml.BuyItem(amount);

                user.WalletAmount -= valueOfTransaction;
                user.AddItem(it, amount);

                _poc.SaveChanges();

                return Ok(user.POI.Select(po => new POItemDTO(po)).ToList());
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Private methods
        private PetOwner GetUser()
        {
            return _poc.GetByEmail(User.Identity.Name);
        }

        private MarketListing GetListingWithID(Market market, int id)
        {
            var list = market.Listings.FirstOrDefault(ml => ml.ID == id);

            if (list == null)
                throw new Exception("We couldn't find the specified listing");

            return list;
        }

        private Market GetMarket()
        {
            return _marketRepo.GetMarket();
        } 
        #endregion
    }
}
