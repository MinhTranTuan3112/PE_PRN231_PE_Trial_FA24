using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Entities;
using Repositories.Interfaces;
using Services.Interfaces;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PremierLeagueAccountService : IPremierLeagueAccountService
    {
        private readonly IConfiguration _configuration;

        private readonly IPremierLeagueAccountRepository _premierLeagueAccountRepository;

        public PremierLeagueAccountService(IConfiguration configuration, IPremierLeagueAccountRepository premierLeagueAccountRepository)
        {
            _configuration = configuration;
            _premierLeagueAccountRepository = premierLeagueAccountRepository;
        }


        public async Task<PremierLeagueAccount> GetAccountByClaims(ClaimsPrincipal claims)
        {
            var userId = claims.FindFirst(c => c.Type == "uid")?.Value;

            if (userId is null)
            {
                throw new UnauthorizedException("Not found user");
            }

            var user = await GetAccountById(Convert.ToInt32(userId));

            if (user is null)
            {
                throw new UnauthorizedException("Not found user");
            }

            return user;

        }

        public async Task<PremierLeagueAccount> GetAccountById(int id)
        {
            var account = await _premierLeagueAccountRepository.FindOneAsync(a => a.AccId == id);

            if (account is null)
            {
                throw new NotFoundException("Account not found");
            }

            return account;
        }

        public async Task<string> Login(string email, string password)
        {
            var account = await _premierLeagueAccountRepository.FindOneAsync(a => a.EmailAddress == email && a.Password == password);

            if (account is null)
            {
                throw new UnauthorizedException("Wrong email or password");
            }

            return CreateToken(account.AccId, account.Role ?? 0);
        }

        private string CreateToken(int uid, int role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? ""));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new(ClaimTypes.Role, role.ToString()),
        new("uid", uid.ToString())
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtSettings:DurationInDays"])
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
