using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CowsAndChicken.DTO;

namespace CowsAndChicken.ApplicationServices
{
    public interface ICowsAndChickenGameService
    {
        Task<Dictionary<int, Stats>> GetEfficiencyStatsAsync(Guid playerId);
        Task<Stats> GetSpeedStatsAsync(Guid playerId);
        Task<Player> RegisterPlayerAsync(Player playerDto);
        Task<GameInfo> StartGameAsync(Guid playerId, int nrOfDigits);
        Task<Result> TakeTurnAsync(Guid playerId, int guess);
    }
}