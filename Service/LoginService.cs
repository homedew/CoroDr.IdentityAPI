using System;
using CoroDr.IdentityAPI.Models;
using CoroDr.IdentityAPI.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CoroDr.IdentityAPI.Service
{
    public class LoginService : ILoginInterface<ApplicationUser>
    {

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public LoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser> FindByUsername(string email)
        {
           return await _userManager.FindByEmailAsync(email);
        }

        public Task SignIn(ApplicationUser user)
        {
            return _signInManager.SignInAsync(user, true);
        }

        public Task SignInAsync(ApplicationUser user, AuthenticationProperties properties, string autheticationMethod = null)
        {
            return _signInManager.SignInAsync(user, properties, autheticationMethod);
        }

        public Task<bool> ValidateCredential(ApplicationUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }
    }
}

