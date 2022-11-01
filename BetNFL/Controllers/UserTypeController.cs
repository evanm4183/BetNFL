using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BetNFL.Repositories;

namespace BetNFL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private readonly IUserTypeRepository _userTypeRepo;

        public UserTypeController(IUserTypeRepository userTypeRepo)
        {
            _userTypeRepo = userTypeRepo;
        }

        [HttpGet]
        public IActionResult GetPublicUserTypes()
        {
            return Ok(_userTypeRepo.GetPublicUserTypes());
        }
    }
}
