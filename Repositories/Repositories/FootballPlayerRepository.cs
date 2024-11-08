using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class FootballPlayerRepository : GenericRepository<FootballPlayer>, IFootballPlayerRepository
    {
        private readonly EnglishPremierLeague2024DbContext _context;

        public FootballPlayerRepository(EnglishPremierLeague2024DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FootballPlayer?> GetFootballPlayerById(string id)
        {
            return await _context.FootballPlayers.AsNoTracking()
                                        .SingleOrDefaultAsync(f => f.FootballPlayerId == id);
        }

        public async Task<List<FootballPlayer>> GetFootballPlayers(string? achievements, string? nominations)
        {
            var query = _context.FootballPlayers.AsNoTracking().Include(f => f.FootballClub).AsQueryable();

            if (!string.IsNullOrEmpty(achievements))
            {
                query = query.Where(f => f.Achievements.ToLower().Contains(achievements.ToLower()));
            }

            if (!string.IsNullOrEmpty(nominations))
            {
                query = query.Where(f => f.Nomination.ToLower().Contains(nominations.ToLower()));
            }

            return await query.ToListAsync();
        }
    }
}
