using Repositories.Entities;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFootballPlayerService
    {
        Task<List<FootballPlayer>> GetFootballPlayers(string? achievements, string? nominations);

        Task<FootballPlayer> GetFootballPlayerById(string id);

        Task<FootballPlayer> CreateFootballPlayer(CreateFootballPlayerRequest request);

        Task UpdateFootballPlayer(FootballPlayer player);

        Task DeletePlayer(string id);
    }
}
