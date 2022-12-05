using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BetNFL.Models;
using BetNFL.Repositories;
using BetNFL.Utils;

namespace BetNFL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileBetController : ControllerBase
    {
        private readonly IUserProfileBetRepository _upBetRepo;
        private readonly IUserProfileRepository _userProfileRepo;

        public UserProfileBetController(IUserProfileBetRepository upBetRepo, IUserProfileRepository userProfileRepo)
        {
            _upBetRepo = upBetRepo;
            _userProfileRepo = userProfileRepo;
        }

        [HttpGet("BettorOpenBets")]
        public IActionResult GetBettorOpenBets()
        {
            var currentUser = AuthUtils.GetCurrentUserProfile(User, _userProfileRepo);
            return Ok(_upBetRepo.GetBettorOpenBets(currentUser.Id));
        }

        [HttpGet("SportsbookOpenBets")]
        public IActionResult GetSportsbookOpenBets()
        {
            var currentUser = AuthUtils.GetCurrentUserProfile(User, _userProfileRepo);
            return Ok(_upBetRepo.GetSportsbookOpenBets(currentUser.Id));
        }

        [HttpGet("BettorSettledBets")]
        public IActionResult GetBettorSettledBets()
        {
            var currentUser = AuthUtils.GetCurrentUserProfile(User, _userProfileRepo);
            return Ok(_upBetRepo.GetBettorSettledBets(currentUser.Id));
        }

        [HttpGet("SportsbookSettledBets")]
        public IActionResult GetSportsbookSettledBets()
        {
            var currentUser = AuthUtils.GetCurrentUserProfile(User, _userProfileRepo);
            return Ok(_upBetRepo.GetSportsbookSettledBets(currentUser.Id));
        }

        [HttpPost]
        public IActionResult PostUserProfileBet(UserProfileBet upBet)
        {
            var currentUser = AuthUtils.GetCurrentUserProfile(User, _userProfileRepo);
            upBet.UserProfileId = currentUser.Id;
            _upBetRepo.PostUserProfileBet(upBet);

            return NoContent();
        }

        [HttpPut("{gameId}")]
        public IActionResult SettleOpenBetsByGame(int gameId)
        {
            _upBetRepo.SettleOpenBetsByGame(gameId);
            return NoContent();
        }
    }
}
