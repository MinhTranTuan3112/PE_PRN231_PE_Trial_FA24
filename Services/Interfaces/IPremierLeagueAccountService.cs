using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPremierLeagueAccountService 
    {
        Task<PremierLeagueAccount> GetAccountByClaims(ClaimsPrincipal claims);

        Task<string> Login(string email, string password);
    }
}
