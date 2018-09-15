using System;

namespace CowsAndChicken.DomainModel
{
    public class GameStartedEventArgs: EventArgs
    {
        public Game Game { get; private set; }

        public GameStartedEventArgs(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }
    }
}
