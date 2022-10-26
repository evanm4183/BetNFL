using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BetNFL.Repositories;
using BetNFL.Models;
using BetNFL.Utils;

namespace BetNFL.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly IBetRepository _betRepo;
        private readonly IUserProfileRepository _userProfileRepo;

        public BetController(IBetRepository betRepo, IUserProfileRepository userProfileRepo)
        {
            _betRepo = betRepo;
            _userProfileRepo = userProfileRepo;
        }

        [HttpGet("{gameId}")]
        public IActionResult GetLiveBetForGame(int gameId)
        {
            var currentUser = AuthUtils.GetCurrentUserProfile(User, _userProfileRepo);

            return Ok(_betRepo.GetLiveBetForGame(currentUser.Id, gameId));
        }

        [HttpPost]
        public IActionResult PostBet(Bet bet)
        {
            var currentUser = AuthUtils.GetCurrentUserProfile(User, _userProfileRepo);

            if (currentUser.UserType.Name == "sportsbook")
            {
                bet.UserProfileId = currentUser.Id;
                _betRepo.PostBet(bet);

                return NoContent();
            }

            return Unauthorized();
        }
    }
}
