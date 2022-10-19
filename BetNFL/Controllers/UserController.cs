using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BetNFL.Repositories;
using BetNFL.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BetNFL.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("DoesUserExist/{firebaseUserId}")]
        public IActionResult DoesUserExist(string firebaseUserId)
        {
            var user = _userRepo.GetByFirebaseUserId(firebaseUserId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("IsAdmin")]
        public IActionResult IsAdmin()
        {
            var currentUser = GetCurrentUserProfile();

            return Ok(currentUser.UserType.Name == "Admin");
        }

        private User GetCurrentUserProfile()
        {
            var firebaseUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _userRepo.GetByFirebaseUserId(firebaseUserId);
        }
    }
}
