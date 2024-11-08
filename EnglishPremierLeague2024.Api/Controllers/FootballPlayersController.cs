using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities;
using Services.Interfaces;
using Shared.Requests;

namespace EnglishPremierLeague2024.Api.Controllers
{
    [Route("api/football-players")]
    [ApiController]
    public class FootballPlayersController : ControllerBase
    {
        private readonly IFootballPlayerService _footballPlayerService;

        public FootballPlayersController(IFootballPlayerService footballPlayerService)
        {
            _footballPlayerService = footballPlayerService;
        }

        [HttpGet, Authorize(Roles = "1, 2")]
        public async Task<ActionResult<List<FootballPlayer>>> GetFootballPlayers([FromQuery] string? achievements, [FromQuery] string? nominations)
        {
            return await _footballPlayerService.GetFootballPlayers(achievements, nominations);
        }

        [HttpGet("{id}"), Authorize(Roles = "1, 2")]
        public async Task<ActionResult<FootballPlayer>> GetFootballPlayerById([FromRoute] string id)
        {
            return await _footballPlayerService.GetFootballPlayerById(id);
        }

        [HttpPost, Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> CreateFootballPlayer([FromBody] CreateFootballPlayerRequest request)
        {
            return Created(nameof(CreateFootballPlayer), await _footballPlayerService.CreateFootballPlayer(request));
        }

        [HttpPut("{id}"), Authorize(Roles = "1")]
        public async Task<ActionResult> UpdateFootballPlayer([FromRoute] string id,  [FromBody] UpdateFootballPlayerRequest request)
        {
            return NoContent();
        }
    }
}
