using Microsoft.AspNetCore.Identity;
using RockPaperScissorsApi.Entities;
using RockPaperScissorsApi.Entities.Models;
using RockPaperScissorsApi.Services.Interfaces;

namespace RockPaperScissorsApi.Services.ConcrateServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;

            _userManager = userManager;
        }

        public async Task<IdentityResult> Register(UserRegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
                return null;
            

            var user = new User { Email = model.Email, UserName = model.UserName };

            var result = await _userManager.CreateAsync(user, model.ConfirmPassword);

            return result;
        }

        public async Task<SignInResult> Login(UserLoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
            return result;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
