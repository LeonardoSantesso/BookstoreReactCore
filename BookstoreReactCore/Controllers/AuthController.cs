using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Models;
using Security.Services.Interfaces;

namespace BookstoreReactCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Signin([FromBody] Login login)
        {
            if (login == null) 
                return BadRequest("Ivalid client request");

            var token = await _loginService.ValidateCredentialsAsync(login);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Token token)
        {
            if (token is null) 
                return BadRequest("Ivalid client request");

            var tokenResult = await _loginService.ValidateCredentialsAsync(token);

            if (tokenResult == null) 
                return BadRequest("Ivalid client request");

            return Ok(tokenResult);
        }


        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;
            var result = await _loginService.RevokeTokenAsync(username);

            if (!result) 
                return BadRequest("Ivalid client request");

            return NoContent();
        }
    }
}
