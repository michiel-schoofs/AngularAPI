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
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PetOwnerController : ControllerBase{
        private readonly IPetOwnerRepository _poRepo;
        private readonly IConfiguration _config;

        public PetOwnerController(IPetOwnerRepository repo,IConfiguration config){
            _poRepo = repo;
            _config = config;
        }

        [HttpGet("image")]
        public ActionResult<ImageDTO> GetImage(){
            var user = _poRepo.GetByEmail(User.Identity.Name);

            if (user.HasImage){
                return new ImageDTO() { Data = user.Image.ToString() };
            }else { 
                return new ImageDTO() { Data = _config.GetValue<string>("DefaultImage") };
            }
        }
    }
}