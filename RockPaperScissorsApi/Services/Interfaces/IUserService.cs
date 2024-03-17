using Microsoft.AspNetCore.Identity;
using RockPaperScissorsApi.Entities.Models;

namespace RockPaperScissorsApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> Register(UserRegisterModel model);

        Task<SignInResult> Login(UserLoginModel model);

        Task Logout();
    }
}
