using System.Collections.Generic;
using System.Linq;

namespace CowsAndChicken.DomainModel
{
    public class PlayerSpeedStats: PlayerStats
    {
        public Stats Stats { get; private set; }

        public PlayerSpeedStats(Player player) : base(player)
        {
            if (player.Games.Any())
            {
                CalculateStats();
            }
        }

        protected override void HandleGameCompleted(Game game)
        {
            CalculateStats();
        }

        private void CalculateStats()
        {
            var timeline = BuildTimeline();
            Stats = new Stats(timeline);
        }

        private IEnumerable<double> BuildTimeline()
        {
            var timeline = new List<double>();

            foreach (var game in CompletedGames)
            {
                var guessStartTime = game.CreatedOn;
                foreach (var guess in game.Turns.OrderBy(turn => turn.CreatedOn))
                {
                    timeline.Add(guess.CreatedOn.Subtract(guessStartTime).TotalMilliseconds);
                    guessStartTime = guess.CreatedOn;
                }
            }

            return timeline;
        }
    }
}
