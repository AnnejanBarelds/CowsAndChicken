using System.Collections.Generic;
using System.Linq;

namespace CowsAndChicken.DomainModel
{
    public class PlayerEfficiencyStats: PlayerStats // Consider implementing IDictionary, for cleanness of the outer interface
    {
        private readonly Dictionary<int, Stats> _stats = new Dictionary<int, Stats>();

        public Dictionary<int, Stats> Stats => new Dictionary<int, Stats>(_stats); // Returning a copy of the list to prevent fiddling from outside the Entity with the actual dictionary

        public PlayerEfficiencyStats(Player player) :base(player)
        {
            if (player.Games.Any())
            {
                Init();
            }
        }

        private void Init()
        {
            var groups = CompletedGames.GroupBy(g => g.NrOfDigits);
            foreach (var group in groups)
            {
                CalculateStats(group.Key, group);
            }
        }

        protected override void HandleGameCompleted(Game game)
        {
            var gamesToRecalculate = CompletedGames.Where(item => item.NrOfDigits == game.NrOfDigits);
            CalculateStats(game.NrOfDigits, gamesToRecalculate);
        }

        private void CalculateStats(int nrOfDigits, IEnumerable<Game> games)
        {
            _stats[nrOfDigits] = new Stats(games.Select(item => item.Turns.Count()));
        }
    }
}
