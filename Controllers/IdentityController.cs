using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoroDr.IdentityAPI.Models;
using CoroDr.IdentityAPI.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoroDr.IdentityAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : Controller
    {
        private Models.IdentityDbContext _identityDbContext;
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private IPasswordHasher<RegistrationViewModel> _passwordHasher;

        private JWTModel _JWTModel;

        public IdentityController(Models.IdentityDbContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, JWTModel jwtModel, IPasswordHasher<RegistrationViewModel> passwordHasher)
        {
            _identityDbContext = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _JWTModel = jwtModel;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("signinbygoogle")]
        public IActionResult SignInWithGoogle()
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("googlesignincallback"));
            return Challenge(properties, "Google");
        }

        [HttpPost("login")]
        public IActionResult Login(string userName, string passWord)
        {
            var isValidUser = IsValidUser(userName, passWord, out var user);

            if(isValidUser)
            {
                var token = GenerateJWTToken(user);
                return Ok(new {Token = token});
            }
            return NotFound();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationViewModel registrationModel)
        {
            if (_identityDbContext.Users.Where(x => x.Username == registrationModel.Username).SingleOrDefault() != null)            {
                return Conflict(new { Message = "Username already exists." });
            }

            var hashedPassword = _passwordHasher.HashPassword(registrationModel, registrationModel.PasswordHash);

            var newUser = new User
            {
                Email = registrationModel.Email,
                AddressName = registrationModel.AddressName,
                Username = registrationModel.Username,
                PasswordHash = hashedPassword
            };

            _identityDbContext.Users.Add(newUser);
            _identityDbContext.SaveChanges();

            var token = GenerateJWTToken(registrationModel);

            return Ok(new { token });
    }


    [HttpPost("googlesignincallback")]
        public async Task<IActionResult> GoogleSignInCallBackAsync()
        {
            var authenticationResult = await HttpContext.AuthenticateAsync("Google");
            if(!authenticationResult.Succeeded)
            {
                return BadRequest("Google Authentication Fail");
            }

            var userFromGoogle = authenticationResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFromSystem = _identityDbContext.Users.Where(x => x.ProviderUserId == userFromGoogle).SingleOrDefault();

            if (userFromSystem == null)
            {
                var email = authenticationResult.Principal.FindFirstValue(ClaimTypes.Email);
                userFromSystem = new User { Email = email, Username = email };
                var user = new RegistrationViewModel { Email = email, Username = email };


                _identityDbContext.Users.Add(userFromSystem);
                var saveResult = await _identityDbContext.SaveChangesAsync();

                if(saveResult >0)
                {
                    // generat the token
                    var token = GenerateJWTToken(user);
                    var signIn = await _signInManager.PasswordSignInAsync(userFromSystem.Username, null, isPersistent: false, lockoutOnFailure: false);
                    if(signIn.Succeeded)
                    {
                        return Ok(new { Token = token });
                    } else
                    {
                        return BadRequest("Fail to SignIn after registration");
                    }
                } else
                {
                    return BadRequest("Google Authentication Fail");
                }
            }

            var userFromDb = _identityDbContext.Users.Where(x => x.ProviderUserId == userFromGoogle).SingleOrDefault();
            var userModel = new RegistrationViewModel { Email = userFromDb.Email, Username = userFromDb.Email };
            //if user already have in system, just generate
            var exisitingToken = GenerateJWTToken(userModel);

            return Ok(new {Token = exisitingToken});
        }

        [HttpPost("signoutgoogle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GoogleSignOut()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        private string GenerateJWTToken(RegistrationViewModel userFromSystem)
        {
            var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userFromSystem.Username),
                new Claim(ClaimTypes.Email, userFromSystem.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWTModel.Key));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var expired = DateTime.Now.AddHours(Convert.ToInt32(_JWTModel.ExpiredTime));

            var token = new JwtSecurityToken(_JWTModel.Issuer, _JWTModel.Audience, claim, signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsValidUser(string userName, string password, out RegistrationViewModel user)
        {
            var userSystem = _identityDbContext.Users.Where(x => x.Username == userName).SingleOrDefault();
            user = new RegistrationViewModel()
            {
                Username = userSystem.Username,
                PasswordHash = userSystem.PasswordHash,
                Email = userSystem.Email
            };
            if (userSystem != null)
            {
                return ValidatePassword(user.PasswordHash, password);
            }

            return false;
        }

        private bool ValidatePassword(string passwordHash, string password)
        {
            using(var hsa256 = SHA256.Create())
            {
                var hashBytes = hsa256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

               return passwordHash == password;
            }
        }
    }
}

