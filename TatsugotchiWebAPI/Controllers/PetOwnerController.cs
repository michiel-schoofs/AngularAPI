#region Imports
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TatsugotchiWebAPI.DTO;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;
#endregion

namespace TatsugotchiWebAPI.Controllers
{
    /// <summary>
    /// Everything to do with pet owners
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PetOwnerController : ControllerBase{
        #region Fields
            private readonly IPetOwnerRepository _poRepo;
            private readonly IImageRepository _imageRepo;
            private readonly IConfiguration _config;
        #endregion

        #region Constructor
            public PetOwnerController(IPetOwnerRepository repo, IConfiguration config
        , IImageRepository imageRepo)
            {

                _poRepo = repo;
                _config = config;
                _imageRepo = imageRepo;
            }
        #endregion

        #region Api methods
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
            [HttpPut("image/update")]
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
        #endregion

        #region Private methods
            private PetOwner GetOwner(){
                return _poRepo.GetByEmail(User.Identity.Name);
            } 
        #endregion
    }
}