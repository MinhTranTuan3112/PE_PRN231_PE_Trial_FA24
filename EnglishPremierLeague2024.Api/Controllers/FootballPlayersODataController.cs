using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Entities;
using Services.Interfaces;

namespace EnglishPremierLeague2024.Api.Controllers
{
    [Route("odata/football-players")]
    [ApiController]
    public class FootballPlayersODataController : ODataController
    {
        private readonly IFootballPlayerService _footballPlayerService;

        public FootballPlayersODataController(IFootballPlayerService footballPlayerService)
        {
            _footballPlayerService = footballPlayerService;
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<IQueryable<FootballPlayer>> GetFootballPlayersWithOData()
        {
            return Ok(_footballPlayerService.GetFootballPlayersQuery());
        }

    }
}
