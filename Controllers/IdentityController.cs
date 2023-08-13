using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoroDr.IdentityAPI.LoginProvider;
using CoroDr.IdentityAPI.Models;
using CoroDr.IdentityAPI.Repository;
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
        private IJWTManageRespositoryInterface _jWTManageRespository;
        private IPasswordHasherRepositoryInterface _passwordHasherRepositoryInterface;
        private IUserRepositoryInterface _userRepositoryInterface;
        private IGoogleInterface _googleInterface;


        public IdentityController(Models.IdentityDbContext context, IJWTManageRespositoryInterface jWTManageRespository, IPasswordHasherRepositoryInterface passwordHasherRepositoryInterface, IUserRepositoryInterface userRepositoryInterface, IGoogleInterface googleInterface)
        {
            _identityDbContext = context;
            _jWTManageRespository = jWTManageRespository;
            _passwordHasherRepositoryInterface = passwordHasherRepositoryInterface;
            _userRepositoryInterface = userRepositoryInterface;
            _googleInterface = googleInterface;

        }

        //[HttpPost("signinbygoogle")]
        //public IActionResult SignInWithGoogle()
        //{
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("googlesignincallback"));
        //    return Challenge(properties, "Google");
        //}

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var user =  _userRepositoryInterface.FindUserByUserName(model.Username);

            if (user.Username != null)
            {
                var hasingPassWordUserLoging = _passwordHasherRepositoryInterface.HashingPassword(model.Password);
                if(hasingPassWordUserLoging.Equals(user.PasswordHash))
                {
                    // return the token
                    var token = _jWTManageRespository.GenerateToken(model.Username);
                    return Ok(new { Token = token });
                }
            }
            return NotFound();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationViewModel registrationModel)
        {
            var user =  _userRepositoryInterface.FindUserByUserName(registrationModel.Username);

            if(user.Username != null)
            {
                return Conflict(new
                {
                    error= "User already exists!",
                    message = "The user you are trying to register already exists in the system."
                });
            }

            var newUser = new User()
            {
                Username = registrationModel.Username,
                PasswordHash = _passwordHasherRepositoryInterface.HashingPassword(registrationModel.Password),
                AddressName = registrationModel.AddressName,
                Email = registrationModel.Username
            };

            var isSaveSucceed = _userRepositoryInterface.SaveChanges(newUser);
            if (isSaveSucceed)
            {

                return Ok("User registered successfully!");

            }

            return BadRequest();
        }

        [HttpPost("loginbyprovider")]
        public Task<IActionResult> LoginAsync([FromBody] LoginViewModel loginModel)
        {
            var isUserNameExist = _userRepositoryInterface.FindUserByUserName(loginModel.Username);

            if(isUserNameExist.Username !=null)
            {
                var token = _jWTManageRespository.GenerateToken(loginModel.Username);
                return Task.FromResult<IActionResult>(Ok(new { Token = token }));
            } else
            {
                var newUser = new User()
                {
                    Username = loginModel.Username,
                    Email = loginModel.Username,
                    ProviderUserId = loginModel.ProviderId
                };

                var isSaveSucceed = _userRepositoryInterface.SaveChanges(newUser);
                if (isSaveSucceed)
                {

                    var token = _jWTManageRespository.GenerateToken(loginModel.Username);
                    return Task.FromResult<IActionResult>(Ok(new { Token = token, Username = loginModel.Username, Email = loginModel.Username }));

                }
                else
                {
                    return Task.FromResult<IActionResult>(BadRequest(new { Error = "Failed to save data." }));
                }
            }
        }


        //[HttpPost("googlesignincallback")]
        //    public async Task<IActionResult> GoogleSignInCallBackAsync()
        //    {
        //        var authenticationResult = await HttpContext.AuthenticateAsync("Google");
        //        if(!authenticationResult.Succeeded)
        //        {
        //            return BadRequest("Google Authentication Fail");
        //        }

        //        var userFromGoogle = authenticationResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        //        var userFromSystem = _identityDbContext.Users.Where(x => x.ProviderUserId == userFromGoogle).SingleOrDefault();

        //        if (userFromSystem == null)
        //        {
        //            var email = authenticationResult.Principal.FindFirstValue(ClaimTypes.Email);
        //            userFromSystem = new User { Email = email, Username = email };
        //            var user = new RegistrationViewModel { Email = email, Username = email };


        //            _identityDbContext.Users.Add(userFromSystem);
        //            var saveResult = await _identityDbContext.SaveChangesAsync();

        //            if(saveResult >0)
        //            {
        //                // generat the token
        //                var token = GenerateJWTToken(user);
        //                var signIn = await _signInManager.PasswordSignInAsync(userFromSystem.Username, null, isPersistent: false, lockoutOnFailure: false);
        //                if(signIn.Succeeded)
        //                {
        //                    return Ok(new { Token = token });
        //                } else
        //                {
        //                    return BadRequest("Fail to SignIn after registration");
        //                }
        //            } else
        //            {
        //                return BadRequest("Google Authentication Fail");
        //            }
        //        }

        //        var userFromDb = _identityDbContext.Users.Where(x => x.ProviderUserId == userFromGoogle).SingleOrDefault();
        //        var userModel = new RegistrationViewModel { Email = userFromDb.Email, Username = userFromDb.Email };
        //        //if user already have in system, just generate
        //        var exisitingToken = GenerateJWTToken(userModel);

        //        return Ok(new {Token = exisitingToken});
        //    }

        //    [HttpPost("signoutgoogle")]
        //    [ProducesResponseType(StatusCodes.Status200OK)]
        //    public async Task<IActionResult> GoogleSignOut()
        //    {
        //        await _signInManager.SignOutAsync();

        //        return Ok();
        //    }

    }
}

