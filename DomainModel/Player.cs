using System;
using System.Collections.Generic;
using System.Linq;

namespace CowsAndChicken.DomainModel
{
    public class Player
    {
        private readonly List<Game> _games = new List<Game>();
        private PlayerEfficiencyStats _efficiency;
        private PlayerSpeedStats _speed;

        private Game CurrentGame
        {
            get
            {
                return _games.OrderBy(g => g.CreatedOn).Last();
            }
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public int Age { get; private set; }

        public PlayerEfficiencyStats Efficiency
        {
            get
            {
                if (_efficiency == null)
                {
                    _efficiency = new PlayerEfficiencyStats(this);
                }
                return _efficiency;
            }
        }

        public PlayerSpeedStats Speed
        {
            get
            {
                if (_speed == null)
                {
                    _speed = new PlayerSpeedStats(this);
                }
                return _speed;
            }
        }

        public event EventHandler<GameStartedEventArgs> GameStarted;

        public event EventHandler GameCompleted;

        public int YearOfBirth
        {
            get
            {
                // This requirement is a bit unclear; we're not able to definitively calculate year of birth based on Age, but Age is all we have
                // TODO: discuss with PO
                return DateTime.Now.AddYears(Age * -1).Year;
            }
        }

        public IEnumerable<Game> Games => _games.ToList(); // Returning a copy of the list to prevent fiddling from outside the Entity with the actual list

        public Player(string name, int age)
        {
            // We can imagine a more restrictive set of allowed ages
            // TODO: discuss with PO
            if (age < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(age), "Value must be a non-negative number");
            }
            Name = name;
            Age = age;
            Id = Guid.NewGuid();
        }

        private void OnGameStarted(GameStartedEventArgs e)
        {
            GameStarted?.Invoke(this, e);
        }

        private void OnGameCompleted(EventArgs e)
        {
            GameCompleted?.Invoke(this, e);
        }

        public Guid StartGame(int nrOfDigits, INumberGenerator numberGenerator)
        {
            // TODO: Check whether a player can start a new game when the previous one isn't finished

            var game = new Game(nrOfDigits, numberGenerator);
            _games.Add(game);
            game.GameCompleted += (o, e) => OnGameCompleted(EventArgs.Empty);
            OnGameStarted(new GameStartedEventArgs(game));
            return game.Id;
        }

        public Outcome TakeTurn(int guess)
        {
            if (CurrentGame == null || CurrentGame.Status != GameStatus.Started)
            {
                throw new InvalidOperationException("No game started yet");
            }
            
            return CurrentGame.ProcessTurn(guess);
        }
    }
}
