using RockPaperScissorsApi.Entities;

namespace RockPaperScissorsApi.Services.Interfaces
{
    public interface IGameService
    {
        Task<bool> MakeTransactionAsync(Guid senderId, Guid receiverId, decimal amount);

        Task<bool> MakeBetTransactionAsync(MatchHistory match);

        Task<MatchHistory> MakeNewMatch(decimal betAmount);

        Task<MatchHistory> ConnectToMatch(MatchHistory match, Guid playerId);

        Task<decimal> GetBalance(Guid userId);

        Task<List<MatchHistory>> GetAwaitingMatches();

        GameResult DetermineResult(GameMoves firstPlayerMove, GameMoves secondPlayerMove);

    }
}
