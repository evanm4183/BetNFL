using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BetNFL.Models;
using BetNFL.Repositories;
using BetNFL.Utils;
using Microsoft.AspNetCore.Authorization;

namespace BetNFL.Controllers
{
    //[Authorize]
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
    }
}
