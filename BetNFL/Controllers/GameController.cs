using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BetNFL.Models;
using BetNFL.Repositories;
using BetNFL.Utils;
using Microsoft.AspNetCore.Authorization;

namespace BetNFL.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _gameRepo;
        private readonly IUserProfileRepository _userProfileRepo;

        public GameController(IGameRepository gameRepo, IUserProfileRepository userProfileRepo)
        {
            _gameRepo = gameRepo;
            _userProfileRepo = userProfileRepo;
        }

        [HttpGet("weeklyGames/{week}")]
        public IActionResult GetAllGamesInWeek(int week)
        {
            return Ok(_gameRepo.GetAllGamesInWeek(week));
        }

        [HttpGet("{id}")]
        public IActionResult GetGameById(int id)
        {
            Game game = _gameRepo.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [HttpPost]
        public IActionResult PostGame(Game game)
        {
            if (AuthUtils.IsCurrentUserAdmin(User, _userProfileRepo))
            {
                _gameRepo.PostGame(game);
                return NoContent();
            }

            return Unauthorized();
        }

        [HttpPut]
        public IActionResult SetScore(Game game)
        {
            if (AuthUtils.IsCurrentUserAdmin(User, _userProfileRepo))
            {
                _gameRepo.SetScore(game);
                return NoContent();
            }

            return Unauthorized();
        }
    }
}
