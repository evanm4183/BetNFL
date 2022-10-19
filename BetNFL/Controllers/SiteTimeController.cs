using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BetNFL.Repositories;
using BetNFL.Models;

namespace BetNFL.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SiteTimeController : ControllerBase
    {
        private readonly ISiteTimeRepository _siteTimeRepo;

        public SiteTimeController(ISiteTimeRepository siteTimeRepo)
        {
            _siteTimeRepo = siteTimeRepo;
        }

        [HttpGet]
        public IActionResult GetSiteTime()
        {
            return Ok(_siteTimeRepo.GetSiteTime());
        }
    }
}
