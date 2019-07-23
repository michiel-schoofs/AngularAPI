using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TatsugotchiWebAPI.DTO;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController:ControllerBase
    {
        public IPetOwnerRepository POR{ get; set; }
        public UserManager<IdentityUser> UM { get; set; }
        private readonly SignInManager<IdentityUser> _sim;
        private readonly IConfiguration _config;

        public UsersController(IPetOwnerRepository por,UserManager<IdentityUser> um
            ,IConfiguration configuration,SignInManager<IdentityUser> sim){

            POR = por;
            UM = um;

            _config = configuration;
            _sim = sim;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDTO loginDTO){
            var user = await UM.FindByNameAsync(loginDTO.Email);

            if (user != null){
               bool canSignIn = await _sim.CanSignInAsync(user);

                if (canSignIn){

                    var res = await _sim.CheckPasswordSignInAsync(user, loginDTO.Password, false);
                    if (res.Succeeded){
                        string token = GetToken(user);
                        return Created("", token);
                    }else
                        ModelState.AddModelError("Error", "The password is incorrect");
                }
                else
                    ModelState.AddModelError("Error", "This user can't sign in");

            }
            else
                ModelState.AddModelError("Error", "We couldn't find the user with that e-mailadres");

            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDTO) {
            if (POR.EmailExists(registerDTO.Email))
                ModelState.AddModelError("Error", "Email is not unique");
            else {
                IdentityUser iu = new IdentityUser(){UserName = registerDTO.Email,Email = registerDTO.Email };
                PetOwner po = new PetOwner(registerDTO);
                var res = await UM.CreateAsync(iu, registerDTO.Password);

                if (res.Succeeded){
                    POR.AddPO(po);
                    POR.SaveChanges();
                    string token = GetToken(iu);
                    return Created("", token);
                }
                else{
                    ModelState.AddModelError("Error", "Something went wrong in the registration process");
                }
            }

            return BadRequest(ModelState);
        }

        private String GetToken(IdentityUser user)
        {
            // Create the token
            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.Email),
              new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              null, null,
              claims,
              expires: DateTime.Now.AddHours(4),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
