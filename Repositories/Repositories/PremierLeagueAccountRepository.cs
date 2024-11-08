using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class PremierLeagueAccountRepository : GenericRepository<PremierLeagueAccount>, IPremierLeagueAccountRepository
    {
        public PremierLeagueAccountRepository(EnglishPremierLeague2024DbContext context) : base(context)
        {
        }
    }
}
