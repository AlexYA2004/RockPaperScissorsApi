using RockPaperScissorsApi.DAL.Interfaces;
using RockPaperScissorsApi.Entities;

namespace RockPaperScissorsApi.DAL.Repositories
{
    public class GameTransactionRepository : IBaseRepository<GameTransaction>
    {
        private readonly AppDbContext _dbContext;

        public GameTransactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(GameTransaction entity)
        {
            await _dbContext.GameTransactions.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<GameTransaction> GetAll()
        {
            return _dbContext.GameTransactions;
        }

        public async Task DeleteAsync(GameTransaction entity)
        {
            _dbContext.GameTransactions.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(GameTransaction entity)
        {
            _dbContext.GameTransactions.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
