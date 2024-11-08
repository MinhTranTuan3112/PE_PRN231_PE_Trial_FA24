using Repositories.Entities;
using Repositories.Interfaces;
using Services.Interfaces;
using Shared.Exceptions;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class FootballPlayerService : IFootballPlayerService
    {
        private readonly IFootballPlayerRepository _footballPlayerRepository;

        private readonly IFootballClubRepository _footballClubRepository;

        public FootballPlayerService(IFootballPlayerRepository footballPlayerRepository, IFootballClubRepository footballClubRepository)
        {
            _footballPlayerRepository = footballPlayerRepository;
            _footballClubRepository = footballClubRepository;
        }

        public async Task<FootballPlayer> CreateFootballPlayer(CreateFootballPlayerRequest request)
        {
            if (await _footballPlayerRepository.AnyAsync(f => f.FootballPlayerId == request.FootballPlayerId))
            {
                throw new BadRequestException("ID already exists");
            }

            if (!await _footballClubRepository.AnyAsync(f => f.FootballClubId == request.FootballClubId))
            {
                throw new BadRequestException("Club id does not exist");
            }

            var player = new FootballPlayer
            {
                FootballPlayerId = request.FootballPlayerId,
                FullName = request.FullName,
                Achievements = request.Achievements,
                Birthday = request.Birthday,
                FootballClubId = request.FootballClubId,
                Nomination = request.Nomination,
                PlayerExperiences = request.PlayerExperiences
            };

            await _footballPlayerRepository.AddAsync(player);

            await _footballPlayerRepository.SaveChangesAsync();

            return await GetFootballPlayerById(player.FootballPlayerId);

        }

        public async Task<FootballPlayer> GetFootballPlayerById(string id)
        {
            var player = await _footballPlayerRepository.GetFootballPlayerById(id);

            if (player is null)
            {
                throw new NotFoundException("Player not found");
            }

            return player;
        }

        public async Task<List<FootballPlayer>> GetFootballPlayers(string? achievements, string? nominations)
        {
            var footballPlayers = await _footballPlayerRepository.GetFootballPlayers(achievements, nominations);

            return footballPlayers;
        }

        public async Task UpdateFootballPlayer(UpdateFootballPlayerRequest request)
        {
            
        }
    }
}
