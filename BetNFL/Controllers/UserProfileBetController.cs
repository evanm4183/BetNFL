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

        [HttpPost]
        public IActionResult PostUserProfileBet(UserProfileBet upBet)
        {
            var currentUser = AuthUtils.GetCurrentUserProfile(User, _userProfileRepo);
            upBet.UserProfileId = currentUser.Id;
            _upBetRepo.PostUserProfileBet(upBet);

            return NoContent();
        }
    }
}
