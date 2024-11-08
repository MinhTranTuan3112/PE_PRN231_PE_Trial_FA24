using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities;
using Services.Interfaces;

namespace EnglishPremierLeague2024.Api.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IPremierLeagueAccountService _premierLeagueAccountService;

        public AccountsController(IPremierLeagueAccountService premierLeagueAccountService)
        {
            _premierLeagueAccountService = premierLeagueAccountService;
        }

        [HttpGet("current-info"), Authorize]
        public async Task<ActionResult<PremierLeagueAccount>> GetCurrentAccountInfo()
        {
            return await _premierLeagueAccountService.GetAccountByClaims(HttpContext.User);
        }
    }
}
