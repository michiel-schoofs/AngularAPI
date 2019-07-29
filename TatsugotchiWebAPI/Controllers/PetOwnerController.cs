#region Imports
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Linq;
using TatsugotchiWebAPI.DTO;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;
using System.Collections.Generic;
#endregion

namespace TatsugotchiWebAPI.Controllers
{
    //Api/PetOwner/Wallet/Daily/Red
    /// <summary>
    /// Everything to do with pet owners
    /// </summary>
    [Route("Api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PetOwnerController : ControllerBase{
        #region Fields
            private readonly IPetOwnerRepository _poRepo;
            private readonly IImageRepository _imageRepo;
            private readonly IListingRepository _listingRepo;
            private readonly IEggRepository _eggRepo;
            private readonly IConfiguration _config;
            private readonly UserManager<IdentityUser> _um;
            private readonly static int DailyRedeemAmount = 80;
        #endregion

        #region Constructor
        public PetOwnerController(IPetOwnerRepository repo, IConfiguration config
                                 , IImageRepository imageRepo, IListingRepository liRepo, IEggRepository eggRepo,
                                  UserManager<IdentityUser> um){
                _poRepo = repo;
                _config = config;
                _imageRepo = imageRepo;
                _eggRepo = eggRepo;
                _listingRepo = liRepo;
                _um = um;
            }
        #endregion

        #region Api methods
            /// <summary>
            /// Get the listings by the current logged in user
            /// </summary>
            /// <returns>A list of listings by the currently logged in user.</returns>
            [HttpGet("Listings/")]
            public ActionResult<ICollection<ListingDTO>> GetListings()
            {
                var listings = _listingRepo.GetListingsByUsers(GetOwner());
                return listings.Select(li => new ListingDTO(li)).ToList();
            }

            /// <summary>
            /// Deletes the current user that's logged in
            /// </summary>
            /// <returns>Ok if the user was succesfully deleted</returns>
            [HttpDelete("Delete")]
            public async Task<ActionResult> DeleteAccount()
            {
                PetOwner po = GetOwner();

                var eggs = _eggRepo.GetEggsByPetOwner(po);
                foreach(var egg in eggs){
                    _eggRepo.RemoveEgg(egg);
                }

                var listings = _listingRepo.GetListingsByUsers(po);
                foreach(var list in listings){
                    _listingRepo.RemoveListing(list);
                }

                if(po.HasImage)
                    _imageRepo.RemoveImage(po.Image);

                _poRepo.SaveChanges();
                _poRepo.RemovePO(po);

                var user = await _um.FindByEmailAsync(po.Email);
                await _um.DeleteAsync(user);

                _poRepo.SaveChanges();

                return Ok();
            }

            /// <summary>
        /// Get the image by the user
        /// </summary>
        /// <returns>Returns a base 64 representing the users image</returns>
            [HttpGet("Image")]
            public ActionResult<ImageDTO> GetImage()
            {
                var user = GetOwner();

                if (user.HasImage)
                {
                    return new ImageDTO() { Data = user.Image.ToString() };
                }
                else
                {
                    return new ImageDTO() { Data = _config.GetValue<string>("DefaultImage") };
                }
            }

            /// <summary>
            /// Updates the users Image 
            /// </summary>
            /// <param name="imageDTO">DTO containing the base64 uri representing the image</param>
            /// <returns>A dto containing the newly made userimage as well as the path to 
            /// <seealso cref="PetOwnerController.GetImage"/>
            /// </returns>
            [HttpPut("Image/Update")]
            public ActionResult<ImageDTO> PostImage(ImageDTO imageDTO)
            {
                Image img = new Image(imageDTO);
                var user = GetOwner();

                //Remove the previous image
                if (user.HasImage)
                {
                    _imageRepo.RemoveImage(user.Image);
                }

                user.Image = img;
                _poRepo.SaveChanges();
                return CreatedAtAction(nameof(GetImage), imageDTO);
            }

            /// <summary>
            /// Get the current logged in user information
            /// </summary>
            /// <returns>Returns a petowner dto based on the current logged in user</returns>
            [HttpGet]
            public ActionResult<PetOwnerDTO> GetUser() {
                var user = GetOwner();
                return new PetOwnerDTO(user);
            }

            /// <summary>
            /// Return the wallet amount of the currently logged in user
            /// </summary>
            /// <returns>the wallet amount of the currently logged in user</returns>
            [HttpGet("Wallet")]
            public ActionResult<object> ViewWalletAmount(){
                var user = GetOwner();
                return new { wallet = user.WalletAmount };
            }

            /// <summary>
            /// Redeem the daily amount of money if the user hasn't redeemd it yet. Otherwise return a bad request.
            /// </summary>
            /// <returns>Returns the petowner dto of the currently logged in user.</returns>
            [HttpPut("Wallet/Daily/Redeem")]
            public ActionResult<PetOwnerDTO> RedeemDailyAmount(){
                try{
                    var user = GetOwner();

                    if( DateTime.Now.Subtract(user.RedeemedMoney).TotalDays < 1)
                        throw new Exception("You already redeemed your daily money amount");

                    user.WalletAmount += DailyRedeemAmount;
                    user.RedeemedMoney = DateTime.Now;

                    _poRepo.SaveChanges();

                    return Ok(new PetOwnerDTO(user));
                } catch (Exception e) {
                    ModelState.AddModelError("Error", e.Message);
                    return BadRequest(ModelState);
                }
            }

            /// <summary>
            /// Get the items in the inventory of the currently logged in user
            /// </summary>
            /// <returns>The items of the currently logged in user</returns>
            [HttpGet("Inventory")]
            public ActionResult<List<ItemDTO>> GetInventory(){
                var user = GetOwner();
                return user.POI.Select(poi => new ItemDTO(poi)).ToList();
            }
        #endregion

        #region Private methods
            private PetOwner GetOwner(){
                return _poRepo.GetByEmail(User.Identity.Name);
            } 
        #endregion
    }
}