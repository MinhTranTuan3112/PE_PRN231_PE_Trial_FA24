using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities;
using Services.Interfaces;
using Shared.Requests;
using Shared.Responses;

namespace EnglishPremierLeague2024.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPremierLeagueAccountService _premierLeagueAccountService;

        public AuthController(IPremierLeagueAccountService premierLeagueAccountService)
        {
            _premierLeagueAccountService = premierLeagueAccountService;
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<AuthResponse>> SignIn([FromBody] SignInRequest request)
        {
            return new AuthResponse
            {
                AccessToken = await _premierLeagueAccountService.Login(request.Email, request.Password)
            };
        }

        [HttpGet("test-admin"), Authorize(Roles = "1")]
        public ActionResult TestAdmin()
        {
            return Ok("Admin");
        }

        [HttpGet("test-staff"), Authorize(Roles = "2")]
        public ActionResult TestStaff()
        {
            return Ok("Staff");
        }


        [HttpGet("test-manager"), Authorize(Roles = "3")]
        public ActionResult TestManager()
        {
            return Ok("Manager");
        }

        [HttpGet("test-customer"), Authorize(Roles = "4")]
        public ActionResult TestCustomer()
        {
            return Ok("Customer");
        }
    }
}
