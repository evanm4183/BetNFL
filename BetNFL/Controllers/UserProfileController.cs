using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BetNFL.Models;
using BetNFL.Repositories;
using BetNFL.Utils;

namespace BetNFL.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _userProfileRepo;

        public UserProfileController(IUserProfileRepository userProfileRepo)
        {
            _userProfileRepo = userProfileRepo;
        }

        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            return Ok(AuthUtils.GetCurrentUserProfile(User, _userProfileRepo));
        }

        [HttpGet("DoesUserExist/{firebaseUserId}")]
        public IActionResult DoesUserExist(string firebaseUserId)
        {
            var user = _userProfileRepo.GetByFirebaseUserId(firebaseUserId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut]
        public IActionResult AddFunds(UserProfile userProfile)
        {
            _userProfileRepo.AddFunds(userProfile);
            return NoContent();
        }

        [HttpGet("IsAdmin")]
        public IActionResult IsAdmin()
        {
            return Ok(AuthUtils.IsCurrentUserAdmin(User, _userProfileRepo));
        }

        [HttpGet("UserType")]
        public IActionResult GetUserType()
        {
            var currentUser = AuthUtils.GetCurrentUserProfile(User, _userProfileRepo);
            return Ok(currentUser.UserType.Name);
        }
    }
}
