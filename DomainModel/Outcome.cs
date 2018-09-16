using System;
using System.Collections.Generic;
using System.Text;

namespace CowsAndChicken.DomainModel
{
    public class Outcome
    {
        public int NrOfCows { get; private set; }

        public int NrOfChickens { get; private set; }

        public bool IsGameCompleted { get; private set; }

        public Outcome(int nrOfCows, int nrOfChickens, bool isGameCompleted)
        {
            if (nrOfCows < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(nrOfCows), "Value cannot be a negative number");
            }

            if (nrOfChickens < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(nrOfChickens), "Value cannot be a negative number");
            }

            NrOfCows = nrOfCows;
            NrOfChickens = nrOfChickens;
            IsGameCompleted = isGameCompleted;
        }
    }
}
