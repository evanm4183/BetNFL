using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BetNFL.Repositories;
using BetNFL.Models;
using BetNFL.Utils;

namespace BetNFL.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SiteTimeController : ControllerBase
    {
        private readonly ISiteTimeRepository _siteTimeRepo;
        private readonly IUserProfileRepository _userProfileRepo;

        public SiteTimeController(ISiteTimeRepository siteTimeRepo, IUserProfileRepository userProfileRepo)
        {
            _siteTimeRepo = siteTimeRepo;
            _userProfileRepo = userProfileRepo;
        }

        [HttpGet]
        public IActionResult GetSiteTime()
        {
            return Ok(_siteTimeRepo.GetSiteTime());
        }

        [HttpPut]
        public IActionResult UpdateSiteTime(SiteTime siteTime)
        {
            if (!AuthUtils.IsCurrentUserAdmin(User, _userProfileRepo))
            {
                return Unauthorized();
            }

            _siteTimeRepo.UpdateSiteTime(siteTime);
            return NoContent();
        }
    }
}
