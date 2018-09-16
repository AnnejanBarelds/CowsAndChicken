using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CowsAndChicken.DomainModel
{
    public class Turn
    {
        public int[] Guess { get; private set; }

        public Outcome Outcome { get; private set; }

        public DateTimeOffset CreatedOn { get; private set; }

        private Turn() { } // To satisfy EF

        public Turn(int[] guess)
        {
            Guess = guess;
            CreatedOn = DateTimeOffset.Now;
        }

        public Outcome Process(int[] numberToGuess)
        {
            int cows = 0;
            int chickens = 0;
            bool correct = true;

            for (int i = 0; i < Guess.Length; i++)
            {
                if (Guess[i].Equals(numberToGuess[i]))
                {
                    cows++;
                }
                else
                {
                    correct = false;
                    if (numberToGuess.Contains(Guess[i]))
                    {
                        chickens++;
                    }
                }
            }

            Outcome = new Outcome(cows, chickens, correct);
            return Outcome;
        }
    }
}
