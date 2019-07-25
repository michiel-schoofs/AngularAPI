using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TatsugotchiWebAPI.DTO;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PetOwnerController : ControllerBase{
        private readonly IPetOwnerRepository _poRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IConfiguration _config;

        public PetOwnerController(IPetOwnerRepository repo,IConfiguration config
            ,IImageRepository imageRepo){

            _poRepo = repo;
            _config = config;
            _imageRepo = imageRepo;
        }

        [HttpGet("image")]
        public ActionResult<ImageDTO> GetImage(){
            var user = GetOwner();

            if (user.HasImage){
                return new ImageDTO() { Data = user.Image.ToString() };
            }else { 
                return new ImageDTO() { Data = _config.GetValue<string>("DefaultImage") };
            }
        }



        [HttpPut("image/update")]
        public ActionResult PostImage(ImageDTO imageDTO){
            Image img = new Image(imageDTO);
            var user = GetOwner();

            //Remove the previous image
            if (user.HasImage) {
                _imageRepo.RemoveImage(user.Image);
            }

            user.Image = img;
            _poRepo.SaveChanges();
            return CreatedAtAction(nameof(GetImage),imageDTO );
        }

        private PetOwner GetOwner(){
           return _poRepo.GetByEmail(User.Identity.Name);
        }
    }
}