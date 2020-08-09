#region Imports
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using TatsugotchiWebAPI.DTO;
    using TatsugotchiWebAPI.Model;
    using TatsugotchiWebAPI.Model.Interfaces; 
#endregion

namespace TatsugotchiWebAPI.Controllers
{
    /// <summary>
    /// Api to register and login a user.
    /// </summary>
    [Route("Api/[Controller]")]
    [ApiController]
    public class UsersController:ControllerBase
    {
        #region Fields
            public IPetOwnerRepository POR { get; set; }
            public UserManager<IdentityUser> UM { get; set; }
            private readonly SignInManager<IdentityUser> _sim;
            private readonly IConfiguration _config;
        #endregion

        #region Constructor
            public UsersController(IPetOwnerRepository por, UserManager<IdentityUser> um
                   , IConfiguration configuration, SignInManager<IdentityUser> sim){
                POR = por;
                UM = um;

                _config = configuration;
                _sim = sim;
            }
        #endregion

        #region Api methods
            /// <summary>
            /// Login using a dto and returns a token that's passed to the front end for authentication
            /// </summary>
            /// <param name="loginDTO">The dto containing the username and the password used for logging in</param>
            /// <returns>A cookie used to identify the user.</returns>
            [AllowAnonymous]
            [HttpPost("Login")]
            public async Task<ActionResult<string>> Login(LoginDTO loginDTO)
            {
                var user = await UM.FindByNameAsync(loginDTO.Email);

                if (user != null)
                {
                    bool canSignIn = await _sim.CanSignInAsync(user);

                    if (canSignIn)
                    {

                        var res = await _sim.CheckPasswordSignInAsync(user, loginDTO.Password, false);
                        if (res.Succeeded)
                        {
                            string token = GetToken(user);
                            return Created("", token);
                        }
                        else
                            ModelState.AddModelError("Error", "The password is incorrect");
                    }
                    else
                        ModelState.AddModelError("Error", "This user can't sign in");

                }
                else
                    ModelState.AddModelError("Error", "We couldn't find the user with that e-mailadres");

                return BadRequest(ModelState);
            }

            /// <summary>
            /// Register the user using a dto containing all the details.Also returns a token that's used to identify the user
            /// </summary>
            /// <param name="registerDTO">A dto containing all the information needed to identify a user.</param>
            /// <returns>A cookie that's passed to the frontend to identify the user.</returns>
            [AllowAnonymous]
            [HttpPost("Register")]
            public async Task<ActionResult<string>> Register(RegisterDTO registerDTO)
            {
                if (POR.EmailExists(registerDTO.Email))
                    ModelState.AddModelError("Error", "Email is not unique");
                else
                {
                    IdentityUser iu = new IdentityUser() { UserName = registerDTO.Email, Email = registerDTO.Email };
                    PetOwner po = new PetOwner(registerDTO);
                    var res = await UM.CreateAsync(iu, registerDTO.Password);

                    if (res.Succeeded)
                    {
                        POR.AddPO(po);
                        POR.SaveChanges();
                        string token = GetToken(iu);
                        return Created("", token);
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Something went wrong in the registration process");
                    }
                }

                return BadRequest(ModelState);
            }
        #endregion

        #region Method to generate the cookie
            private String GetToken(IdentityUser user)
            {
                // Create the token
                var claims = new[]
                {
                  new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                  new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dsjfksldezeteiurhroijeo123489321564sqdfeijziofjzoe"));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                  null, null,
                  claims,
                  expires: DateTime.Now.AddHours(4),
                  signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            } 
        #endregion
    }
}
