using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CowsAndChicken.DomainModel
{
    public class Game
    {
        private int[] _numberToGuess; // Not readonly to satisfy EF
        private readonly List<Turn> _turns = new List<Turn>();

        public Guid Id { get; private set; }

        public int NrOfDigits { get; private set; }

        public IEnumerable<Turn> Turns => _turns.ToList(); // Returning a copy of the list to prevent fiddling from outside the Entity with the actual list

        public GameStatus Status { get; private set; }

        public DateTimeOffset CreatedOn { get; private set; }

        public event EventHandler GameCompleted;

        private Game() { } // To satisfy EF

        public Game(int nrOfDigits, INumberGenerator numberGenerator)
        {
            // Consider refactoring nrOfDigits into its own class, because that would result in consolidating argument checking that now happens in two places
            if (nrOfDigits < 4 || nrOfDigits > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(nrOfDigits), "Value must be between 4 and 8");
            }

            NrOfDigits = nrOfDigits;
            _numberToGuess = ConvertToArray(numberGenerator.Generate(nrOfDigits));

            Id = Guid.NewGuid();
            Status = GameStatus.Started;
            CreatedOn = DateTimeOffset.Now;
        }

        public Outcome ProcessTurn(int guess)
        {
            if (Status == GameStatus.Completed)
            {
                throw new InvalidOperationException("No turns can be taken on a completed game");
            }
            if (NrOfDigits != guess.ToString().Length)
            {
                throw new ArgumentOutOfRangeException(nameof(guess), $"Guess must be exactly {NrOfDigits} in length");
            }

            var guessArray = ConvertToArray(guess);

            var turn = new Turn(guessArray);

            _turns.Add(turn);

            var result = turn.Process(_numberToGuess);

            if (result.IsGameCompleted)
            {
                Status = GameStatus.Completed;
                OnGameCompleted(EventArgs.Empty);
            }

            return turn.Outcome;
        }

        private int[] ConvertToArray(int i)
        {
            return Array.ConvertAll(i.ToString().ToArray(), x => int.Parse(x.ToString()));
        }

        private void OnGameCompleted(EventArgs e)
        {
            GameCompleted?.Invoke(this, e);
        }
    }
}
