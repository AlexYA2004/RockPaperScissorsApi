using RockPaperScissorsApi.DAL.Interfaces;
using RockPaperScissorsApi.Entities;

namespace RockPaperScissorsApi.DAL.Repositories
{
    public class MatchHistoryRepository : IBaseRepository<MatchHistory>
    {
        private readonly AppDbContext _dbContext;

        public MatchHistoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(MatchHistory entity)
        {
            await _dbContext.MatchHistories.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<MatchHistory> GetAll()
        {
            return _dbContext.MatchHistories;
        }

        public async Task DeleteAsync(MatchHistory entity)
        {
            _dbContext.MatchHistories.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(MatchHistory entity)
        {
            _dbContext.MatchHistories.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
