using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BetNFL.Models;
using BetNFL.Repositories;
using BetNFL.Utils;

namespace BetNFL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _userProfileRepo;

        public UserProfileController(IUserProfileRepository userProfileRepo)
        {
            _userProfileRepo = userProfileRepo;
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

        [HttpPost]
        public IActionResult RegisterNewUser(UserProfile userProfile)
        {
            _userProfileRepo.RegisterNewUser(userProfile);
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
