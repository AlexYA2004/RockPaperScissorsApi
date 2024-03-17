using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RockPaperScissorsApi.DAL.Interfaces;
using RockPaperScissorsApi.DAL.Repositories;
using RockPaperScissorsApi.Entities;
using RockPaperScissorsApi.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace RockPaperScissorsApi.Services.ConcrateServices
{
    public class GameService : IGameService
    {
        private IBaseRepository<GameTransaction> _gameTransactionRepository;

        private IBaseRepository<MatchHistory> _matchHistoryRepository;

        private IBaseRepository<User> _userRepository;

        public GameService(IBaseRepository<GameTransaction> gameTransactionRepository,
                           IBaseRepository<MatchHistory> matchHistoryRepository,
                           IBaseRepository<User> userRepository)
        {
            _gameTransactionRepository = gameTransactionRepository;

            _matchHistoryRepository = matchHistoryRepository;

            _userRepository = userRepository;
        }

        public async Task<bool> MakeTransactionAsync(Guid senderId, Guid receiverId, decimal amount)
        {
            var sender = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == senderId);

            var receiver = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == receiverId);

            if (sender == null || receiver == null)
            {
                return false;
            }

            if (sender.Balance < amount)
            {
                return false;
            }

            sender.Balance -= amount;

            receiver.Balance += amount;

            var transaction = new GameTransaction
            {
                SenderId = senderId,

                ReceiverId = receiverId,

                Amount = amount
            };

            await _gameTransactionRepository.CreateAsync(transaction);

            await _userRepository.UpdateAsync(sender);

            await _userRepository.UpdateAsync(receiver);

            return true;
        }

        public async Task<bool> MakeBetTransactionAsync(MatchHistory match)
        {
            var firstPlayer = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == match.Player1Id);

            var secondPlayer = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == match.Player2Id);

            if (match.Result == GameResult.FirstPlayerWin)
            {
                firstPlayer.Balance += match.BetAmount;

                secondPlayer.Balance -= match.BetAmount;
            }
            else if (match.Result == GameResult.SecondPlayerWin)
            {
                secondPlayer.Balance += match.BetAmount;

                firstPlayer.Balance -= match.BetAmount;
            }
            else 
            {
                firstPlayer.Balance += 0;

                secondPlayer.Balance += 0;
            }

            var betTransaction = new GameTransaction()
            {
                Amount = match.BetAmount,
                ReceiverId = match.Result == GameResult.FirstPlayerWin ? match.Player1Id :
                              match.Result == GameResult.SecondPlayerWin ? match.Player2Id : Guid.Empty,
                SenderId = match.Result == GameResult.FirstPlayerWin ? match.Player2Id :
                            match.Result == GameResult.SecondPlayerWin ? match.Player1Id : Guid.Empty,
                MatchId = match.Id
            };

            await _gameTransactionRepository.CreateAsync(betTransaction);

            await _userRepository.UpdateAsync(firstPlayer);

            await _userRepository.UpdateAsync(secondPlayer);

            await _matchHistoryRepository.CreateAsync(match);

            return true; 
        }

        public async Task<MatchHistory> MakeNewMatch(decimal betAmount)
        {
            var newMatch = new MatchHistory()
            {
                BetAmount = betAmount
            };

            return newMatch;
        }


        public async Task<MatchHistory> ConnectToMatch(MatchHistory match, Guid playerId)
        {
            if (match.Player1Id == Guid.Empty)
            {
                match.Player1Id = playerId;
            }
            else if (match.Player2Id == Guid.Empty)
            {
                match.Player2Id = playerId;
            }
          
            return match;

        }

        public async Task<decimal> GetBalance(Guid userId)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return -1;

            return user.Balance;
        }

        public async Task<List<MatchHistory>> GetAwaitingMatches()
        {
            var matches = await _matchHistoryRepository.GetAll()
                .Where(m => m.Player1Id == Guid.Empty || m.Player2Id == Guid.Empty)
                .ToListAsync();

            return matches;
        }

        public GameResult DetermineResult(GameMoves firstPlayerMove, GameMoves secondPlayerMove)
        {
            if (firstPlayerMove == secondPlayerMove)
            {
                return GameResult.Draw;
            }
            else if ((firstPlayerMove == GameMoves.К && secondPlayerMove == GameMoves.Н) ||
                     (firstPlayerMove == GameMoves.Н && secondPlayerMove == GameMoves.Б) ||
                     (firstPlayerMove == GameMoves.Б && secondPlayerMove == GameMoves.К))
            {
                return GameResult.FirstPlayerWin;
            }
            else
            {
                return GameResult.SecondPlayerWin;
            }
        }
    }
}
