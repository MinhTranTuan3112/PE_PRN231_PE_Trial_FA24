using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class FootballClubRepository : GenericRepository<FootballClub>, IFootballClubRepository
    {
        public FootballClubRepository(EnglishPremierLeague2024DbContext context) : base(context)
        {
        }
    }
}
