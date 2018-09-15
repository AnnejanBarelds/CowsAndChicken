using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CowsAndChicken.DomainModel
{
    public abstract class PlayerStats
    {
        private readonly Player _player;

        protected IEnumerable<Game> CompletedGames => _player.Games.Where(game => game.Status == GameStatus.Completed);

        public PlayerStats(Player player)
        {
            _player = player;
            player.GameStarted += (p, gsea) => gsea.Game.GameCompleted += (g, ea) => HandleGameCompleted(g as Game);
        }

        protected abstract void HandleGameCompleted(Game game);
    }
}
