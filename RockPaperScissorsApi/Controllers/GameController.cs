using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RockPaperScissorsApi.DAL.Repositories;
using RockPaperScissorsApi.Entities;
using RockPaperScissorsApi.Services.ConcrateServices;
using RockPaperScissorsApi.Services.Interfaces;
using System.Security.Claims;

namespace RockPaperScissorsApi.Controllers
{
    [ApiController]
    [Route("RockPaperScissors/[controller]")]
    public class GameController : ControllerBase
    {
        private IGameService _gameService;

        public static List<MatchHistory> awaitingMatches = new List<MatchHistory>();

        public static List<MatchHistory> playingMatches = new List<MatchHistory>();

        public GameController( IGameService gameService)
        {       
            _gameService = gameService;
        }

        [Authorize]
        [HttpPost("create_new_match")] 
        public async Task<ActionResult> CreateNewMatch([FromBody] decimal betAmount)
        {
            try
            {
                var newMatch = await _gameService.MakeNewMatch(betAmount);

                if (newMatch == null)
                {
                    return BadRequest("Unable to create a new match.");
                }

                awaitingMatches.Add(newMatch);

                return Ok(newMatch);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.InnerException}");
            }
        }

        [Authorize]
        [HttpPost("connect_to_match")]
        public async Task<ActionResult> ConnectToMatch([FromBody] Guid matchId )
        {
            var matchToConnect = awaitingMatches.FirstOrDefault(m => m.Id == matchId);

            if (matchToConnect == null)
                return NotFound();

            var playerId =  Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _gameService.ConnectToMatch(matchToConnect, playerId);

            if (result.Player1Id != Guid.Empty && result.Player2Id != Guid.Empty)
            {
                awaitingMatches.Remove(matchToConnect);

                playingMatches.Add(result);
            }
            else
            {
                awaitingMatches.Remove(matchToConnect);

                awaitingMatches.Add(result);
            }

            return Ok(result);

        }

        [Authorize]
        [HttpGet("awating_matches")]
        public async Task<ActionResult> GetAwatingMatches()
        {
            return Ok(awaitingMatches);
        }

        [Authorize]
        [HttpPost("play_match")] 
        public async Task<ActionResult> PlayMatch(GameMoves firstPlayerMove, GameMoves secondPlayerMove)
        {
            var playerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var matchToPlay = playingMatches.FirstOrDefault(m => m.Player1Id == playerId || m.Player2Id == playerId);

            var gameResult = _gameService.DetermineResult(firstPlayerMove, secondPlayerMove);

            matchToPlay.Result = gameResult;

            await _gameService.MakeBetTransactionAsync(matchToPlay);

            return Ok(gameResult);
        }
    }
}
