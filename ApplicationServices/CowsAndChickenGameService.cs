using CowsAndChicken.DomainModel;
using CowsAndChicken.DTO;
using CowsAndChicken.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CowsAndChicken.ApplicationServices
{
    public class CowsAndChickenGameService : ICowsAndChickenGameService
    {
        private readonly INumberGenerator _numberGenerator;
        private readonly ICowsAndChickenContext _ctx;
        private static readonly List<DomainModel.Player> _activePlayers = new List<DomainModel.Player>(); // TODO: replace with proper cache that handles expiration etc

        public CowsAndChickenGameService(ICowsAndChickenContext ctx) :this(ctx, new NumberGenerator())
        {
            // The default constructor allows the CowsAndChickenGameService to function as a proper Application Service, i.e. a service that can be used to interact with the Domain Model without
            // requiring any knowlegde of how that Domain Model works. That is why we are explicitly providing a way to instantiate CowsAndChickenGameService without having to provide an INumberGenerator
        }

        public CowsAndChickenGameService(ICowsAndChickenContext ctx, INumberGenerator numberGenerator)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _numberGenerator = numberGenerator ?? throw new ArgumentNullException(nameof(numberGenerator));
        }

        public async Task<DTO.Player> RegisterPlayerAsync(DTO.Player playerDto)
        {
            //TODO: Use Automapper

            var player = new DomainModel.Player(playerDto.Name, playerDto.Age);
            await _ctx.Players.AddAsync(player);
            await _ctx.SaveChangesAsync();
            _activePlayers.Add(player);

            return new DTO.Player
            {
                Name = player.Name,
                Age = player.Age,
                YearOfBirth = player.YearOfBirth,
                Id = player.Id
            };
        }

        public async Task<GameInfo> StartGameAsync(Guid playerId, int nrOfDigits)
        {
            var player = await GetPlayerAsync(playerId);
            var gameId = player.StartGame(nrOfDigits, _numberGenerator);
            await _ctx.SaveChangesAsync();

            return new GameInfo
            {
                GameId = gameId,
                PlayerName = player.Name,
                PlayerYearOfBirth = player.YearOfBirth
            };
        }

        public async Task<Result> TakeTurnAsync(Guid playerId, int guess)
        {
            var player = await GetPlayerAsync(playerId);
            var outcome = player.TakeTurn(guess);
            await _ctx.SaveChangesAsync();

            // TODO: Use Automapper
            return new Result
            {
                NrOfChickens = outcome.NrOfChickens,
                NrOfCows = outcome.NrOfCows,
                IsGameCompleted = outcome.IsGameCompleted
            };
        }

        public async Task<Dictionary<int,DTO.Stats>> GetEfficiencyStatsAsync(Guid playerId)
        {
            var player = await GetPlayerAsync(playerId);
            var stats = new Dictionary<int, DTO.Stats>();

            foreach (var item in player.Efficiency.Stats)
            {
                stats[item.Key] = new DTO.Stats
                {
                    Average = item.Value.Average,
                    Max = item.Value.Max,
                    Mean = item.Value.Mean,
                    Median = item.Value.Median,
                    Min = item.Value.Min
                };
            };

            return stats;
        }

        public async Task<DTO.Stats> GetSpeedStatsAsync(Guid playerId)
        {
            var player = await GetPlayerAsync(playerId);
            return new DTO.Stats
            {
                Average = player.Speed.Stats.Average,
                Max = player.Speed.Stats.Max,
                Mean = player.Speed.Stats.Mean,
                Median = player.Speed.Stats.Median,
                Min = player.Speed.Stats.Min
            };
        }

        private async Task<DomainModel.Player> GetPlayerAsync(Guid playerId)
        {
            var player = _activePlayers.SingleOrDefault(item => item.Id == playerId);
            if (player != null)
            {
                _ctx.Players.Attach(player);
            }
            else
            {
                player = await _ctx.Players.Include(p => p.Games).ThenInclude(g => g.Turns).SingleAsync(p => p.Id == playerId);
                _activePlayers.Add(player);
            }
            return player;
        }
    }
}
