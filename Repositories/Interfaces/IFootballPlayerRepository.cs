using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IFootballPlayerRepository : IGenericRepository<FootballPlayer>
    {
        Task<List<FootballPlayer>> GetFootballPlayers(string? achievements, string? nominations);

        Task<FootballPlayer?> GetFootballPlayerById(string id);
    }
}
