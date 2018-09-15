using System;
using System.Collections.Generic;
using System.Text;

namespace CowsAndChicken.DomainModel
{
    public class Outcome
    {
        public int NrOfCows { get; private set; }

        public int NrOfChicken { get; private set; }

        public bool IsGameCompleted { get; private set; }

        public Outcome(int nrOfCows, int nrOfChicken, bool isGameCompleted)
        {
            if (nrOfCows < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(nrOfCows), "Value cannot be a negative number");
            }

            if (nrOfChicken < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(nrOfChicken), "Value cannot be a negative number");
            }

            NrOfCows = nrOfCows;
            NrOfChicken = nrOfChicken;
            IsGameCompleted = isGameCompleted;
        }
    }
}
