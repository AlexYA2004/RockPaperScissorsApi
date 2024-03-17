using RockPaperScissorsApi.DAL.Interfaces;
using RockPaperScissorsApi.DAL.Repositories;
using RockPaperScissorsApi.Entities;
using RockPaperScissorsApi.Services.ConcrateServices;
using RockPaperScissorsApi.Services.Interfaces;

namespace RockPaperScissorsApi
{
    public static class Initializer
    {
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IGameService, GameService>();
        }

        public static void InitializeRepositories(this IServiceCollection services) 
        {
            services.AddScoped<IBaseRepository<User>, UserRepository>();

            services.AddScoped<IBaseRepository<MatchHistory>, MatchHistoryRepository>();

            services.AddScoped<IBaseRepository<GameTransaction>, GameTransactionRepository>();
        }
    }
}
