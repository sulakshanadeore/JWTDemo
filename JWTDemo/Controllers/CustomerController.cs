using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            return Ok("This is protected data. You are authorized!");
        }


        [HttpGet("showtime")]
        public IActionResult ShowTime()
        {
            return Content(DateTime.Now.ToShortTimeString());
        }
    }

}
